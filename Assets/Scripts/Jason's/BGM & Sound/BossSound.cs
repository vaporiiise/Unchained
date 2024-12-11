using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bosssound : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource attackSource;
    public AudioSource idleSource;
    public AudioSource jumpSource;
    public AudioSource hurtSource;
    public AudioSource landingSource;    // New landing sound source
    public AudioSource walkingSource;   // New walking sound source
    public AudioSource growlingSource;  // New growling sound source
    public AudioSource groundPoundSource; // New ground pound sound source

    [Header("Audio Clips")]
    public AudioClip[] attackClips;
    public AudioClip[] idleClips;
    public AudioClip[] jumpClips;
    public AudioClip[] hurtClips;
    public AudioClip[] landingClips;
    public AudioClip[] walkingClips;
    public AudioClip[] growlingClips;
    public AudioClip[] groundPoundClips;  // New ground pound sound clips

    [Header("Idle Sound Settings")]
    public float idleSoundInterval = 5f;
    private float idleTimer;

    public Animator bossAnim;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject HealthBar;

    void Update()
    {
        HandleIdleSounds();
    }

    public void PlayAttackSound()
    {
        PlayRandomSound(attackSource, attackClips);
    }

    public void PlayJumpSound()
    {
        PlayRandomSound(jumpSource, jumpClips);
    }

    public void PlayHurtSound()
    {
        PlayRandomSound(hurtSource, hurtClips);
    }

    public void PlayLandingSound()
    {
        PlayRandomSound(landingSource, landingClips);
    }

    public void PlayWalkingSound()
    {
        PlayRandomSound(walkingSource, walkingClips);
    }

    public void PlayGrowlingSound()
    {
        PlayRandomSound(growlingSource, growlingClips);
    }

    public void PlayGroundPoundSound() // New method for Ground Pound sound
    {
        PlayRandomSound(groundPoundSource, groundPoundClips);
    }

    private void HandleIdleSounds()
    {
        if (idleClips.Length == 0 || idleSource.isPlaying) return;

        idleTimer += Time.deltaTime;
        if (idleTimer >= idleSoundInterval)
        {
            PlayRandomSound(idleSource, idleClips);
            idleTimer = 0f;
        }
    }

    private void PlayRandomSound(AudioSource source, AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        int randomIndex = Random.Range(0, clips.Length);
        source.clip = clips[randomIndex];
        source.Play();
    }
}
