using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Crowd : MonoBehaviour
{
    [SerializeField] private Transform[] peopleGroups; 
    [SerializeField] private float minJumpHeight = 1f; 
    [SerializeField] private float maxJumpHeight = 3f; 
    [SerializeField] private float minJumpDuration = 0.3f; 
    [SerializeField] private float maxJumpDuration = 0.7f; 
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float maxDelay = 0.3f;

    private void Start()
    {
        peopleGroups = GetComponentsInChildren<Transform>(false).Where(t => t != transform).ToArray();
        StartJumpingAnimation();
    }

    private void StartJumpingAnimation()
    {
        for (int i = 0; i < peopleGroups.Length; i++)
        {
            Transform currentGroup = peopleGroups[i];
            bool isEvenGroup = i % 2 == 0;

            // Randomize jump duration and height for each group
            float randomJumpHeight = Random.Range(minJumpHeight, maxJumpHeight);
            float randomJumpDuration = Random.Range(minJumpDuration, maxJumpDuration);
            float randomDelay = Random.Range(0f, maxDelay);

            Vector3 upPosition = currentGroup.localPosition + new Vector3(0, randomJumpHeight, 0);
            Vector3 downPosition = currentGroup.localPosition;

            if (isEvenGroup)
            {
                // Use random delay and speed
                currentGroup.DOLocalMove(upPosition, randomJumpDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetDelay(randomDelay);
            }
            else
            {
                // Set initial position slightly up and use random delay and speed
                currentGroup.localPosition = new Vector3(currentGroup.localPosition.x,
                    currentGroup.localPosition.y + randomJumpHeight, currentGroup.localPosition.z);
                
                currentGroup.DOLocalMove(downPosition, randomJumpDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo).SetDelay(randomDelay);
            }
        }
    }
}
