using _Project._Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum state
{
    accelerating,
    maxSpeed,
    idle
}
public class CarAudio : MonoBehaviour
{
    [SerializeField] private List<AudioClip> takeDamageSounds;
    [SerializeField] private AudioClip destroyed;

    [SerializeField] private List<AudioClip> accelerateSounds;
    [SerializeField] private List<AudioClip> maxSpeedSounds;
    [SerializeField] private List<AudioClip> breakSounds;
    [SerializeField] private List<AudioClip> turnSounds;
    [SerializeField] private List<AudioClip> idleSounds;

    AudioSource audioSource;

    state engineState = state.idle;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        var carStats = GetComponent<CarStats>();
        var carController = GetComponent<Controller>();
        carStats.takeDamage += DamageAudio;
        //carStats.destroyed += DestroyedAudio;

        carController.accelerateEvent += AccelerateAudio;
        carController.maxSpeedEvent += MaxSpeedEvent;
        carController.breakEvent += BreakAudio;
        carController.turnEvent += TurnAudio;
        carController.idleEvent += IdleAudio;
    }

    private void IdleAudio()
    {
        if (engineState == state.idle) return;
        engineState = state.idle;

        audioSource.clip = RandomSound(idleSounds);
        audioSource.Play();
    }

    private void TurnAudio()
    {
        AudioSource.PlayClipAtPoint(RandomSound(turnSounds), transform.position);
    }

    private void BreakAudio()
    {
        engineState = state.idle;

        audioSource.clip = RandomSound(breakSounds);
        audioSource.Play();

    }

    private void MaxSpeedEvent()
    {
        if (engineState == state.maxSpeed) return;
        engineState = state.maxSpeed;

        audioSource.clip = RandomSound(maxSpeedSounds);
        audioSource.Play();
    }

    private void AccelerateAudio()
    {
        if (engineState == state.accelerating) return;
        engineState = state.accelerating;

        audioSource.clip = RandomSound(accelerateSounds);
        audioSource.Play();
    }

    private void DamageAudio(TakeDamageEventObj obj)
    {
        AudioSource.PlayClipAtPoint(RandomSound(takeDamageSounds), transform.position);
    }

    AudioClip RandomSound(List<AudioClip> clips)
    {
        return clips[UnityEngine.Random.Range(0, clips.Count)];
    }
}
