using System;
using UnityEngine;
using UnityEngine.Events;

namespace _Project._Scripts
{
    public class CarStats : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        [Header("Resistance")]
        public float resistance;
        [Header("Damage")]
        [SerializeField] private float damage = 20;        
        [SerializeField] private float damageMitigationModifier = 1000;        
        [SerializeField] private float collisionCooldown = 0.2f; // Cooldown to prevent multiple damage calculations
        [Header("PowerUp")]
        public float abilityRechargeRate;

        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private Material deadMaterial;
        [SerializeField] public MeshRenderer renderer;
        
        private Rigidbody _rigidbody;
        private Controller _carController;

        [HideInInspector] public bool _isDestroyed;
        private float _lastCollisionTime = -1f;

        public Action<TakeDamageEventObj> takeDamage;
        public static event Action destroyed;
        public Action destroyedLocal;
        
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _carController = GetComponent<Controller>();
            
            Reset();
        }

        // Update is called once per frame
        void Update()
        {
            if (_isDestroyed)
            {
                return;
            }
        }

        public void TakeDamage(float damageReceived)
        {
            if (_isDestroyed)
            {
                return;
            }
            
            currentHealth -= damageReceived - resistance * damageReceived / 100;
            takeDamage?.Invoke(new TakeDamageEventObj(damageReceived));
            if (currentHealth <= 0)
            {
                Destroy();
            }
        }
        

        public void DealDamage(Collision collision, bool isBumper)
        {
            if (Time.time - _lastCollisionTime < collisionCooldown) return; // Prevent repeated collisions
            _lastCollisionTime = Time.time;

            CarStats otherCar = collision.gameObject.GetComponent<CarStats>();
            if (otherCar == null)
            {
                return;
            }
            
            // Get the relative velocity of the two cars
            Vector3 relativeVelocity = collision.relativeVelocity;

            // Calculate the speed of the impact
            float impactSpeed = relativeVelocity.magnitude;

            // Get the direction of impact by normalizing the relative velocity
            Vector3 impactDirection = relativeVelocity.normalized;

            // Get the angle of impact (dot product between impact direction and normal of collision)
            float impactAngle = Vector3.Dot(impactDirection, collision.contacts[0].normal);

            // Normalize the angle to a value between 0 (glancing hit) and 1 (direct hit)
            float angleFactor = Mathf.Abs(impactAngle);

            // Use mass and speed to calculate base damage
            float baseDamage = _rigidbody.mass * impactSpeed * damage / damageMitigationModifier;

            // Scale damage based on impact angle
            float finalDamage = baseDamage * angleFactor;

            if (finalDamage > damage)
            {
                finalDamage = damage;
            }
            
            if (isBumper)
            {
                finalDamage /= 2;
            }
            // print(finalDamage);
            otherCar.TakeDamage(finalDamage);
        }
        
        public void DealDamage(Collider other, bool isBumper)
        {
            if (Time.time - _lastCollisionTime < collisionCooldown) return; // Prevent repeated collisions
            _lastCollisionTime = Time.time;

            CarStats otherCar = other.gameObject.GetComponent<CarStats>();
            if (otherCar == null)
            {
                return;
            }

            // Retrieve Rigidbody component from the other car
            Rigidbody otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody == null)
            {
                return; // No rigidbody, can't calculate damage
            }

            // Get the relative velocity between the two cars
            Vector3 relativeVelocity = _rigidbody.velocity - otherRigidbody.velocity;

            // Calculate the speed of the impact
            float impactSpeed = relativeVelocity.magnitude;

            // Get the direction of impact by normalizing the relative velocity
            Vector3 impactDirection = relativeVelocity.normalized;

            // Get the angle of impact (assume the collision normal is pointing towards the current object)
            Vector3 collisionNormal = other.transform.position - transform.position;
            float impactAngle = Vector3.Dot(impactDirection, collisionNormal.normalized);

            // Normalize the angle to a value between 0 (glancing hit) and 1 (direct hit)
            float angleFactor = Mathf.Abs(impactAngle);

            // Use mass and speed to calculate base damage
            float baseDamage = _rigidbody.mass * impactSpeed * damage / damageMitigationModifier;

            // Scale damage based on impact angle
            float finalDamage = baseDamage * angleFactor;

            if (finalDamage > damage)
            {
                finalDamage = damage;
            }

            if (isBumper)
            {
                finalDamage /= 2;
            }

            // Apply damage to the other car
            otherCar.TakeDamage(finalDamage);
        }

        public void Reset()
        {
            currentHealth = maxHealth;
        }

        private void Destroy()
        {
            _carController.enabled = false;
            explosionParticles.Play();
            _isDestroyed = true;
            renderer.material = deadMaterial;
            destroyedLocal?.Invoke();
            destroyed?.Invoke();
        }
    }
}
