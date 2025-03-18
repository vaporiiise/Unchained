using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class SFXmanager : MonoBehaviour
{
    [Header("Audio Mixer Settings")]
    [SerializeField] private AudioMixer myMixer; // Assign the same AudioMixer as GameplayVolumeManager
    [SerializeField] private string sfxMixerParameter = "SFX"; // Name of the SFX parameter in the mixer

    [Header("Scripts with SFX")]
    [SerializeField] private MonoBehaviour[] sfxScripts; // Scripts that contain AudioSources

    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        FindAudioSources();
        ApplyMixerToSFX();
        DebugMixerGroup();
        UpdateSFXVolume(); // Sync with saved volume
    }

    private void FindAudioSources()
    {
        foreach (MonoBehaviour script in sfxScripts)
        {
            if (script == null)
            {
                Debug.LogError("SFXmanager: One of the assigned scripts is missing!");
                continue;
            }

            AudioSource[] foundSources = script.GetComponentsInChildren<AudioSource>();
            if (foundSources.Length == 0)
            {
                Debug.LogWarning($"SFXmanager: No AudioSource found in {script.name}");
                continue;
            }

            audioSources.AddRange(foundSources);
        }
    }

    private void ApplyMixerToSFX()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource == null) continue;

            if (audioSource.outputAudioMixerGroup == null)
            {
                Debug.LogWarning($"SFXmanager: AudioSource in {audioSource.gameObject.name} had no Mixer assigned.");
                // Assign the SFX mixer group
                audioSource.outputAudioMixerGroup = myMixer.FindMatchingGroups("SFX")[0];
            }
            else
            {
                Debug.Log($"SFXmanager: {audioSource.gameObject.name} is already assigned to {audioSource.outputAudioMixerGroup.name}");
            }
        }
    }

    private void DebugMixerGroup()
    {
        if (audioSources.Count == 0)
        {
            Debug.LogError("SFXmanager: No AudioSources found to debug!");
            return;
        }

        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.outputAudioMixerGroup == null)
            {
                Debug.LogError($"SFXmanager: {audioSource.gameObject.name} is NOT assigned to an Audio Mixer Group!");
            }
            else
            {
                Debug.Log($"SFXmanager: {audioSource.gameObject.name} is correctly assigned to: {audioSource.outputAudioMixerGroup.name}");
            }
        }
    }

    public void UpdateSFXVolume()
    {
        if (myMixer == null)
        {
            Debug.LogError("SFXmanager: No Audio Mixer assigned!");
            return;
        }

        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        myMixer.SetFloat(sfxMixerParameter, Mathf.Log10(Mathf.Max(savedVolume, 0.0001f)) * 20);
        Debug.Log($"SFXmanager: Updated SFX volume to {savedVolume}");
    }
}
