using _Project._Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    [SerializeField] private AudioClip takeDamage;
    [SerializeField] private AudioClip destroyed;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        var carStats = GetComponent<CarStats>();
        carStats.takeDamage += DamageAudio;
        //carStats.destroyed += DestroyedAudio;
        audioSource = GetComponent<AudioSource>();
    }

    private void DestroyedAudio(DestroyedEventObj obj)
    {
        audioSource.clip = destroyed;
        audioSource.Play();
    }

    private void DamageAudio(TakeDamageEventObj obj)
    {
        audioSource.clip = takeDamage;
        audioSource.Play();
    }
}
