using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioFinder : MonoBehaviour
{
    public AudioClip targetClip; // Assign the SFX you are looking for in Inspector

    void Update()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audio in allAudioSources)
        {
            if (audio.isPlaying && audio.clip == targetClip)
            {
                Debug.Log("🔊 Playing: " + targetClip.name + " on " + audio.gameObject.name, audio.gameObject);
            }
        }
    }
}
