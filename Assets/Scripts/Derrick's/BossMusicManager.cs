using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicManager : MonoBehaviour
{
    public static BossMusicManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource backgroundMusicSource;
    [SerializeField] private AudioSource dialogueMusicSource;
    
    [Header("Settings")]
    [SerializeField] private float fadeDuration = 1.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayDialogueMusic()
    {
        StartCoroutine(FadeOutMusic(backgroundMusicSource));
        StartCoroutine(FadeInMusic(dialogueMusicSource));
    }

    public void StopDialogueMusic()
    {
        StartCoroutine(FadeOutMusic(dialogueMusicSource));
        StartCoroutine(FadeInMusic(backgroundMusicSource));
    }

    private IEnumerator FadeOutMusic(AudioSource source)
    {
        if (source.isPlaying)
        {
            float startVolume = source.volume;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                source.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
                yield return null;
            }
            source.Stop();
            source.volume = startVolume;
        }
    }

    private IEnumerator FadeInMusic(AudioSource source)
    {
        if (!source.isPlaying)
        {
            source.volume = 0f;
            source.Play();
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                source.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
                yield return null;
            }
        }
    }

    public void OnDialogueStart()
    {
        PlayDialogueMusic();
    }

    public void OnDialogueEnd()
    {
        StopDialogueMusic();
    }
}
