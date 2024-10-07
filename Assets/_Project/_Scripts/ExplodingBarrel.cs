using System;
using UnityEngine;

namespace _Project._Scripts
{
    public class ExplodingBarrel : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private GameObject visuals;

        private MeshCollider _collider;

        private void Start()
        {
            _collider = GetComponent<MeshCollider>();

            Reset();
        }

        private void Reset()
        {
            visuals.SetActive(true);
            _collider.enabled = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
           var carStats = collision.gameObject.GetComponent<CarStats>();
           
           if (carStats)
           {
               explosionParticles.Play();
           
               visuals.SetActive(false);
               _collider.enabled = false;
               
               carStats.TakeDamage(damage);
           }
        }
    }
}
