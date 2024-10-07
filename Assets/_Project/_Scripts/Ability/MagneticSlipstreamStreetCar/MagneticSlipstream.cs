using System.Collections;
using System.Collections.Generic;
using _Project._Scripts.Ability;
using AI;
using UnityEngine;

public class MagneticSlipstream : Ability
{
    public float pullForce = 500f; 
    public float slipstreamBoost = 10f;
    public float spawnHeight = 5f; 
    public float bulletSpeed = 10f; 
    public float moveDuration = 2f; 
    private bool isSlipstreamActive = false;
    private GameObject nearestEnemy;
    private List<MeleeEnemyInput> enemyCars;
    private Rigidbody rb;

    private void Start()
    {
        enemyCars = new List<MeleeEnemyInput>(FindObjectsOfType<MeleeEnemyInput>());
        rb = GetComponent<Rigidbody>();
    }

    public override void ActivateAbility()
    {
        nearestEnemy = FindNearestEnemyCar();
        if (nearestEnemy != null)
        {
            isSlipstreamActive = true;

            StartCoroutine(SpawnAndBurstTowardsEnemy()); // Use a coroutine for movement
        }
    }

    private GameObject FindNearestEnemyCar()
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (MeleeEnemyInput enemy in enemyCars)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.gameObject;
            }
        }

        return closestEnemy;
    }

    private IEnumerator SpawnAndBurstTowardsEnemy()
    {
        rb.velocity = Vector3.zero;

        Vector3 spawnPosition = transform.position + Vector3.up * spawnHeight;

        transform.position = spawnPosition;

        Vector3 targetPosition = nearestEnemy.transform.position;
        
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / moveDuration;
            Vector3 newPosition = Vector3.Lerp(spawnPosition, targetPosition, t);

            transform.position = newPosition;

            Vector3 directionToEnemy = (targetPosition - transform.position).normalized;
            RotateTowardsEnemy(directionToEnemy);

            yield return null;
        }

        transform.position = targetPosition;
        
        StartCoroutine(BounceAfterReachingTarget());
        
        isSlipstreamActive = false;
    }
    
    private IEnumerator BounceAfterReachingTarget()
    {
        float bounceDuration = 0.5f; 
        float bounceHeight = 1f; 
        float elapsedTime = 0f;

        Vector3 originalPosition = transform.position; // Store the original position

        while (elapsedTime < bounceDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / bounceDuration;
            float bounce = Mathf.Sin(t * Mathf.PI) * bounceHeight; 

            transform.position = new Vector3(originalPosition.x, originalPosition.y + bounce, originalPosition.z - bounce);
            
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

            yield return null;
        }

        transform.position = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z);
    }

    private void RotateTowardsEnemy(Vector3 directionToEnemy)
    {
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
        transform.rotation = targetRotation;
    }
}
