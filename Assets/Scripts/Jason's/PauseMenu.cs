using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI; // Reference to the pause menu UI
    public AudioSource backgroundMusic; // Reference to the AudioSource playing the music

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.P))
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
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        Time.timeScale = 0f; // Pause the game time
        GameIsPaused = true; // Update the game state
        backgroundMusic.Pause(); // Pause the background music
    }
}
