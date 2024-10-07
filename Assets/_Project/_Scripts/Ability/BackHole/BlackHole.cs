using UnityEngine;

namespace _Project._Scripts.Ability.BackHole
{
    public class BlackHole : MonoBehaviour
    {
        private float expansionSpeed;
        private float maxRadius;
        private float time;
        private GameObject triggeringCar;

        private MeshRenderer sphereRenderer;
        private Vector3 initialScale;
        private float currentScale = 1f;
        
        public void Initialize(float expansionSpeed, float maxRadius, float time, GameObject triggeringCar)
        {
            this.expansionSpeed = expansionSpeed;
            this.maxRadius = maxRadius;
            this.time = time;
            this.triggeringCar = triggeringCar;
            
            initialScale = transform.localScale;
        }

        private void Update()
        {
            ExpandSphere();
        }

        private void ExpandSphere()
        {
            // Expand the sphere as long as it's below the max size
            if (currentScale < maxRadius)
            {
                currentScale += expansionSpeed * Time.deltaTime;

                // Update the sphere's transform scale to grow the object
                transform.localScale = initialScale * currentScale;

                // Increase the collider radius to match the visual size of the sphere
                // sphereCollider.radius = currentScale;

                // Once the sphere reaches the maximum size, destroy the object and disable the renderer
                if (currentScale >= maxRadius)
                {
                    sphereRenderer.enabled = false; // Hide the sphere after the effect ends
                    Destroy(gameObject); // Destroy the sphere after reaching max size
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == triggeringCar) return; // Ignore the car that triggered the ability

            CarStats car = other.GetComponent<CarStats>();
            if (car == null) return;
            
            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb == null) return;
            
            // Calculate direction from the blast center to the object
            Vector3 forceDirection = (other.transform.position - transform.position).normalized;
            // rb.AddForce(forceDirection * blastForce, ForceMode.Impulse); // Apply force to the object
        }
    }
}
