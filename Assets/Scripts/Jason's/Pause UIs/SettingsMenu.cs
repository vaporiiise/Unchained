using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the background music volume slider
    public AudioSource backgroundMusic; // Reference to the background music AudioSource

    public Slider sfxSlider; // Reference to the SFX volume slider
    public AudioSource[] soundEffects; // Array of AudioSources for SFX

    private const string VolumeKey = "BackgroundVolume"; // Key for saving background volume in PlayerPrefs
    private const string SfxVolumeKey = "SFXVolume"; // Key for saving SFX volume in PlayerPrefs

    private void Start()
    {
        // Initialize the background music volume
        if (backgroundMusic != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumeKey, 1f); // Default to 1 (max volume)
            backgroundMusic.volume = savedVolume; // Set the background music volume
            volumeSlider.value = savedVolume; // Initialize the slider value
            volumeSlider.onValueChanged.AddListener(SetBackgroundVolume);
        }

        // Initialize the SFX volume
        float savedSfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 1f); // Default to 1 (max volume)
        SetSfxVolume(savedSfxVolume); // Set the SFX volumes
        sfxSlider.value = savedSfxVolume; // Initialize the SFX slider value
        sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    public void SetBackgroundVolume(float volume)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = volume; // Set the volume based on slider value
            PlayerPrefs.SetFloat(VolumeKey, volume); // Save the background volume to PlayerPrefs
            PlayerPrefs.Save(); // Ensure the data is saved immediately
        }
    }

    public void SetSfxVolume(float volume)
    {
        // Save the SFX volume to PlayerPrefs
        PlayerPrefs.SetFloat(SfxVolumeKey, volume);
        PlayerPrefs.Save(); // Ensure the data is saved immediately

        // Set the volume for each SFX AudioSource if they exist
        if (soundEffects != null)
        {
            foreach (AudioSource sfx in soundEffects)
            {
                if (sfx != null) // Check if the SFX AudioSource is not null
                {
                    sfx.volume = volume; // Set the volume for each SFX AudioSource
                }
            }
        }
    }

    // Make this method public to allow access from the PauseMenu script
    public void CloseSettingsPanel()
    {
        PauseMenu pauseMenu = FindObjectOfType<PauseMenu>();
        if (pauseMenu != null)
        {
            pauseMenu.CloseSettingsPanel(); // Close the settings panel and show pause menu
        }
    }
}
