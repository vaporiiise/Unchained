using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Slider volumeSlider; // Reference to the volume slider
    public AudioSource backgroundMusic; // Reference to the background music AudioSource

    private void Start()
    {
        // Initialize the slider value with the current volume
        volumeSlider.value = backgroundMusic.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        backgroundMusic.volume = volume; // Set the volume based on slider value
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
