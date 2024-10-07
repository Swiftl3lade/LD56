using UnityEngine;

namespace _Project._Scripts.Ability.Blast
{
    public class Blast : Ability
    {
        public GameObject blastSpherePrefab;
        public float expansionSpeed = 10f;  
        public float maxRadius = 5f;       
        public float blastForce = 500f;

        // Call this function to activate the blast
        public override void ActivateAbility()
        {
            GameObject blastSphere = Instantiate(blastSpherePrefab, transform.position, Quaternion.identity);
            blastSphere.transform.SetParent(transform);
            BlastSphere sphereScript = blastSphere.GetComponent<BlastSphere>();
            sphereScript.Initialize(expansionSpeed, maxRadius, blastForce, gameObject);
        }
    }
}
