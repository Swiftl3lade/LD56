using UnityEngine;

namespace _Project._Scripts.Ability.BackHole
{
    public class ThrowBlackHole : Ability
    {
        public GameObject blackHolePrefab;
        public Transform blackHoleSpawner;
        public float throwForce = 1000f;
        public float expansionSpeed = 10f;  
        public float maxRadius = 5f;       
        public float pullForce = 1000f;       
        public float time = 5f;

        // Call this function to activate the blast
        public override void ActivateAbility()
        {
            GameObject blackHole = Instantiate(blackHolePrefab, blackHoleSpawner.position, Quaternion.identity);
            blackHole.GetComponent<Rigidbody>().AddForce(throwForce * transform.forward);
            
            BlackHole blackHoleScript = blackHole.GetComponent<BlackHole>();
            blackHoleScript.Initialize(pullForce, expansionSpeed, maxRadius, time, gameObject);
        }
    }
}
