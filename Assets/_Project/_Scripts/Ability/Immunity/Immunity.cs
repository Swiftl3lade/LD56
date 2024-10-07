using Unity.VisualScripting;
using UnityEngine;

namespace _Project._Scripts.Ability.Immunity
{
    public class Immunity : Ability
    {
        [SerializeField] private GameObject immunityPrefab;
        [SerializeField] private float duration;

        private GameObject immunity;
        private CarStats carStats;
        private float originalResistance;

        public override void Start()
        {
            base.Start();
            carStats = GetComponent<CarStats>();
            originalResistance = carStats.resistance;
        }

        // Update is called once per frame
        public override void ActivateAbility()
        {
            immunity = Instantiate(immunityPrefab, transform.position, transform.rotation);

            immunity.transform.SetParent(transform);
            carStats.resistance = 100;
            //rb.velocity = transform.TransformDirection(localVelocity);
            Invoke(nameof(RevertImmunity), duration);
        }

        private void RevertImmunity()
        {
            carStats.resistance = originalResistance;
            Destroy(immunity);
        }
    }
}
