using UnityEngine;

namespace _Project._Scripts.Ability.BarrelThow
{
    public class BarrelThrow : Ability
    {
        public GameObject barrelPrefab;
        public Transform barrelSpawner;
        public float throwForce = 10f;

        // Call this function to activate the blast
        public override void ActivateAbility()
        {
            GameObject barrel = Instantiate(barrelPrefab, barrelSpawner.position, Quaternion.identity);
            barrel.GetComponent<Rigidbody>().AddForce(throwForce * -transform.forward);
        }
    }
}
