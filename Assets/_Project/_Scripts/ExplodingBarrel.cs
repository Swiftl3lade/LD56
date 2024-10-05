using System;
using UnityEngine;

namespace _Project._Scripts
{
    public class ExplodingBarrel : MonoBehaviour
    {
        [SerializeField] private float damage;

        private void OnCollisionEnter(Collision collision)
        {
           var carStats = collision.gameObject.GetComponent<CarStats>();

           if (carStats)
           {
               carStats.TakeDamage(damage);
           }
        }
    }
}
