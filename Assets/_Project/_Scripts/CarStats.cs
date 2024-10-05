using UnityEngine;

namespace _Project._Scripts
{
    public class CarStats : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        [Header("Endurance")]
        [SerializeField] private float endurance;
        [SerializeField] private float enduranceModifier = 1;
        [Header("Speed")]
        [SerializeField] private float maxSpeed;
        [SerializeField] private float currentSpeed;
        [SerializeField] private float speedModifier = 1;
        [Header("Damage")]
        [SerializeField] private float damage;
        [Header("PowerUp")]
        [SerializeField] private float powerUpRate;

        private bool _isDestroyed;
        
        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isDestroyed)
            {
                return;
            }
            
            if (currentHealth <= 0)
            {
                _isDestroyed = true;
            }
        }

        public void TakeDamage(float damageReceived)
        {
            currentHealth = damageReceived - endurance * enduranceModifier;
            print(currentHealth);
        }

        public float DealDamage()
        {
            return damage * currentSpeed * speedModifier;
        }
    }
}
