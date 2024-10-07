using UnityEngine;

namespace _Project._Scripts.Ability.Blast
{
    public class Blast : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ApplyBlast();
            }
        }

        public void ApplyBlast()
        {
            
        }
    }
}
