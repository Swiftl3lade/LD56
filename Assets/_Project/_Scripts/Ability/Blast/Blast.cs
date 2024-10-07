using UnityEngine;

namespace _Project._Scripts.Ability.Blast
{
    public class Blast : MonoBehaviour
    {
        public GameObject blastSpherePrefab;
        public float expansionSpeed = 10f;  
        public float maxRadius = 5f;       
        public float blastForce = 500f;     

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ActivateBlast();
            }
        }
        
        // Call this function to activate the blast
        public void ActivateBlast()
        {
            GameObject blastSphere = Instantiate(blastSpherePrefab, transform.position, Quaternion.identity);
            blastSphere.transform.SetParent(transform);
            BlastSphere sphereScript = blastSphere.GetComponent<BlastSphere>();
            sphereScript.Initialize(expansionSpeed, maxRadius, blastForce, gameObject);
        }
    }
}
