using System;
using UnityEngine;

namespace _Project._Scripts
{
    public class CarBumper : MonoBehaviour
    {
        private GameObject _parent;
        private CarStats _stats;

        private void Awake()
        {
            _parent = transform.parent.gameObject;
            _stats = _parent.GetComponent<CarStats>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision == null) return;
            if (collision.collider.CompareTag("FrontBumper"))
            {
                _stats.DealDamage(collision, true);

                return;
            }

            _stats.DealDamage(collision, false);
        }
    }
}
