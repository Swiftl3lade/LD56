using System;
using UnityEngine;

namespace _Project._Scripts.Ability.BackHole
{
    public class BlackHole : MonoBehaviour
    {
        private float expansionSpeed;
        private float maxRadius;
        private float pullForce;
        private float time;
        private GameObject triggeringCar;
        private bool isExpanding = false;

        private Vector3 initialScale;
        private float currentScale = 1f;
        
        public void Initialize(float pullForce, float expansionSpeed, float maxRadius, float time, GameObject triggeringCar)
        {
            this.pullForce = pullForce;
            this.expansionSpeed = expansionSpeed;
            this.maxRadius = maxRadius;
            this.time = time;
            this.triggeringCar = triggeringCar;
            
            initialScale = transform.localScale;
        }

        private void Update()
        {
            if (isExpanding)
            {
                ExpandSphere();
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!isExpanding)
            {
                isExpanding = true;
                GetComponent<Rigidbody>().isKinematic = true;
            }
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
                    isExpanding = false;
                    Destroy(gameObject, time); // Destroy the sphere after reaching max size
                }
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == triggeringCar) return; // Ignore the car that triggered the ability

            CarStats car = other.GetComponent<CarStats>();
            if (car == null) return;
            
            Rigidbody rb = car.GetComponent<Rigidbody>();
            if (rb == null) return;
            
            // Apply force towards the orb's center
            Vector3 pullDirection = (transform.position - other.transform.position).normalized;
            rb.AddForce(pullDirection * pullForce * Time.deltaTime, ForceMode.Acceleration);
        }
    }
}
