using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI; // Reference to the pause menu UI
    public AudioSource backgroundMusic; // Reference to the AudioSource playing the music
    private AudioLowPassFilter lowPassFilter; // Reference to the low-pass filter

    [SerializeField]
    private KeyCode pauseKey = KeyCode.Escape; // Configurable key for pausing the game

    void Start()
    {
        // Get the AudioLowPassFilter component from the background music AudioSource
        if (backgroundMusic != null)
        {
            lowPassFilter = backgroundMusic.GetComponent<AudioLowPassFilter>();
        }
    }

    void Update()
    {
        // Check if the configured pause key is pressed
        if (Input.GetKeyDown(pauseKey))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        Time.timeScale = 1f; // Resume the game time
        GameIsPaused = false; // Update the game state

        backgroundMusic.UnPause(); // Resume the background music

        if (lowPassFilter != null)
        {
            lowPassFilter.enabled = false; // Disable the filter to reset to normal sound
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        Time.timeScale = 0f; // Pause the game time
        GameIsPaused = true; // Update the game state

        backgroundMusic.Pause(); // Pause the background music

        if (lowPassFilter != null)
        {
            lowPassFilter.enabled = true; // Enable the filter
            lowPassFilter.cutoffFrequency = 800; // Lower the cutoff to muffle the sound
        }
    }
}
