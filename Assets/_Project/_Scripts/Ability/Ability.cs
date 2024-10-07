using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Project._Scripts.Ability
{
    public class Ability : MonoBehaviour
    {
        public float botAbilityMaxDelay = 5;
        
        private Image abilityBar;
        
        public bool isPlayer;
        private CarStats _carStats;
        private float currentCharge;
        
        [HideInInspector] public bool hasBotActivatedAbility;
        [HideInInspector] public bool canActivateAbility;
        
        // Start is called before the first frame update
        void Start()
        {
            abilityBar = AbilityBarReference.Instance.GetComponent<Image>();
            _carStats = GetComponent<CarStats>();
        }

        private void UpdateAbilityBar()
        {
            if (isPlayer)
            {
                abilityBar.fillAmount = currentCharge;
            }
        }

        private void Update()
        {
            RechargeAbility();
            
            if (canActivateAbility)
            {
                if (Input.GetKeyDown(KeyCode.Space) && isPlayer)
                {
                    ActivateAbility();
                    currentCharge = 0f;
                    UpdateAbilityBar();
                    canActivateAbility = false;
                }
                else if (!isPlayer && !hasBotActivatedAbility)
                {
                    StartCoroutine(ActivateAbilityCoroutine());
                }
            }
        }

        IEnumerator ActivateAbilityCoroutine()
        {
            hasBotActivatedAbility = true;
            yield return new WaitForSeconds(Random.Range(0, botAbilityMaxDelay));
            ActivateAbility();
            currentCharge = 0f;
            UpdateAbilityBar();
            canActivateAbility = false;
            hasBotActivatedAbility = false;
        }

        private void RechargeAbility()
        {
            // Increase the charge based on the car's recharge rate
            if (canActivateAbility) return;
            
            currentCharge += _carStats.abilityRechargeRate/100 * Time.deltaTime;
            // currentCharge = Mathf.Clamp(currentCharge, 0f, 100f);  // Clamp between 0 and 100

            // Update the slider UI to reflect the current charge
            UpdateAbilityBar();

            // print(currentCharge);
                
            // Check if the ability is fully charged
            if (currentCharge >= 1)
            {
                canActivateAbility = true;
            }
        }

        public virtual void ActivateAbility()
        {
            
        }
    }
}
