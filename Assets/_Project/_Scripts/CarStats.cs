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
        [Header("Endurance")]
        [SerializeField] private float endurance;
        [SerializeField] private float enduranceModifier = 1;
        [Header("Speed")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float speedModifier = 1;
        [Header("Damage")]
        [SerializeField] private float damage;
        [Header("PowerUp")]
        [SerializeField] private float powerUpRate;

        [SerializeField] private ParticleSystem explosionParticles;

        private Rigidbody _rigidbody;
        private CarController _carController;
        private bool _isDestroyed;

        public Action<TakeDamageEventObj> takeDamage;
        public Action<DestroyedEventObj> destroyed;
        
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _carController = GetComponent<CarController>();
            
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
            currentHealth -= damageReceived - endurance * enduranceModifier;
            takeDamage?.Invoke(new TakeDamageEventObj(damageReceived));
            if (currentHealth <= 0)
            {
                Destroy();
            }
        }

        public float DealDamage()
        {
            return damage * currentSpeed * speedModifier;
        }

        public void Reset()
        {
            currentHealth = maxHealth;
            _rigidbody.constraints = RigidbodyConstraints.None;
        }

        private void Destroy()
        {
            _carController.enabled = false;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            explosionParticles.Play();
            _isDestroyed = true;
            destroyed?.Invoke(new DestroyedEventObj());
        }
    }
}
