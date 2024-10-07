using System;
using UnityEngine;

namespace _Project._Scripts.Ability.Blast
{
    public class BlastSphere : MonoBehaviour
    {
        private float expansionSpeed;
        private float maxRadius;
        private float blastForce;
        private GameObject triggeringCar;

        // private SphereCollider sphereCollider;
        private MeshRenderer sphereRenderer; // Reference to the Renderer component
        private Vector3 initialScale; // Store initial scale
        private float currentScale = 1f; // Start with a scale of 1

        private void Start()
        {
            // sphereCollider = GetComponent<SphereCollider>();
        }

        public void Initialize(float expansionSpeed, float maxRadius, float blastForce, GameObject triggeringCar)
        {
            this.expansionSpeed = expansionSpeed;
            this.maxRadius = maxRadius;
            this.blastForce = blastForce;
            this.triggeringCar = triggeringCar;
            
            sphereRenderer = GetComponent<MeshRenderer>();
            initialScale = transform.localScale;

            sphereRenderer.enabled = false; // Renderer is disabled initially
        }

        private void Update()
        {
            ExpandSphere();
        }

        private void ExpandSphere()
        {
            // Enable the renderer while the sphere expands
            if (!sphereRenderer.enabled)
            {
                sphereRenderer.enabled = true;
            }

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
            rb.AddForce(forceDirection * blastForce, ForceMode.Impulse); // Apply force to the object
        }
    }
}