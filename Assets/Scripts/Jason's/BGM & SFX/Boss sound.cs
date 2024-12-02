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

    [Header("Audio Clips")]
    public AudioClip[] attackClips;      
    public AudioClip[] idleClips;        
    public AudioClip[] jumpClips;        
    public AudioClip[] hurtClips;        

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
