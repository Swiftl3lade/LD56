using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project._Scripts
{
    public class DeformableCar : MonoBehaviour
    {
        // car Mesh
        public MeshFilter carMeshFilter;

        // deform Force
        public float deformationForce = 100f;

        // deformation Smoothness
        public float deformationSmoothness = 0.5f;

        [Range(0.05f, 3f)]
        // affected area
        public float deformDist = 2f;

        // transform List
        public List<Transform> deformationTargets;

        // Vertex deformation Limit
        public int maxDeformationsPerVertex = 3;

        private Vector3[] _originalVertices;
        private Dictionary<int, int> _deformationCounts;

        void Start()
        {
            if (carMeshFilter != null)
            {
                _originalVertices = carMeshFilter.mesh.vertices;
                _deformationCounts = new Dictionary<int, int>(_originalVertices.Length);
            }
        }

        // void Update()
        // {
        //     //for testing
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         ResetDeformation();
        //     }
        // }

        // apply deformation
        public void ApplyDeformation(Vector3 contactPoint, Vector3 contactNormal)
        {
            if (deformationTargets == null || deformationTargets.Count == 0)
                return;

            Transform closestTarget = FindClosestTarget(contactPoint);
            Mesh mesh = carMeshFilter.mesh;
            Vector3[] vertices = mesh.vertices;
            Vector3 localContactPoint = carMeshFilter.transform.InverseTransformPoint(contactPoint);
            Vector3 localDeformationTarget = carMeshFilter.transform.InverseTransformPoint(closestTarget.position);

            for (int i = 0; i < vertices.Length; i++)
            {
                // checks if max deformation reached
                if (_deformationCounts.ContainsKey(i) && _deformationCounts[i] >= maxDeformationsPerVertex)
                {
                    continue;
                }

                float distanceToContact = Vector3.Distance(vertices[i], localContactPoint);
                // if (distanceToContact <= deformDist*.001f)
                if (distanceToContact <= deformDist)
                {
                    float deformationAmount = Mathf.Clamp01(1 - distanceToContact / deformationSmoothness);

                    // Calculate deformation direction based on the closest transform
                    Vector3 deformationDirection = (localDeformationTarget - vertices[i]).normalized;

                    // Apply deformation towards the closest transform
                    vertices[i] += deformationDirection * deformationAmount * deformationForce/100 * Time.deltaTime;

                    // Update deformation count for this vertex
                    if (!_deformationCounts.ContainsKey(i))
                    {
                        _deformationCounts[i] = 0;
                    }
                    _deformationCounts[i]++;
                }
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
        }

        // Method to find the closest transform to the contact point
        private Transform FindClosestTarget(Vector3 contactPoint)
        {
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (Transform target in deformationTargets)
            {
                float distance = Vector3.Distance(contactPoint, target.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }

        // Method to handle collisions
        private void OnCollisionEnter(Collision collision)
        {
            Vector3 collisionPoint = collision.contacts[0].point;
            Vector3 collisionNormal = collision.contacts[0].normal;

            // Apply deformation to the car
            ApplyDeformation(collisionPoint, collisionNormal);
        }

        // Method to reset deformation
        public void ResetDeformation()
        {
            if (carMeshFilter != null && _originalVertices != null)
            {
                Mesh mesh = carMeshFilter.mesh;
                mesh.vertices = _originalVertices;
                mesh.RecalculateNormals();

                // Reset deformation counts
                _deformationCounts.Clear();
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var target in deformationTargets)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(target.transform.position, 0.2f);
            }
        }
    }
}