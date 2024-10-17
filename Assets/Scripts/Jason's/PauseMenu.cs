using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused { get; private set; }

    public GameObject pauseMenuUI; // Reference to the pause menu UI
    public GameObject settingsPanel; // Reference to the settings panel

    public GameObject player; // Reference to the player GameObject

    [SerializeField]
    private KeyCode pauseKey = KeyCode.Escape; // Configurable key for pausing the game

    private MonoBehaviour[] playerScripts; // To store the player scripts for enabling/disabling

    void Start()
    {
        // Ensure the settings panel is hidden at the start
        settingsPanel.SetActive(false);

        // Get all the MonoBehaviour scripts attached to the player
        if (player != null)
        {
            playerScripts = player.GetComponents<MonoBehaviour>();
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

    public static void TogglePause()
    {
        GameIsPaused = !GameIsPaused;

        // Optionally manage the game time scale
        Time.timeScale = GameIsPaused ? 0f : 1f;
    }

    void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        settingsPanel.SetActive(false); // Ensure settings panel is hidden
        Time.timeScale = 1f; // Resume the game time
        GameIsPaused = false; // Update the game state

        EnablePlayerScripts(); // Re-enable all player scripts
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        settingsPanel.SetActive(false); // Ensure settings panel is hidden
        Time.timeScale = 0f; // Pause the game time
        GameIsPaused = true; // Update the game state

        DisablePlayerScripts(); // Disable all player scripts
    }

    // Method to open the settings panel
    public void OpenSettingsPanel()
    {
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        settingsPanel.SetActive(true); // Show the settings panel
    }

    // Method to close the settings panel and return to the pause menu
    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false); // Hide the settings panel
        pauseMenuUI.SetActive(true); // Show the pause menu UI
    }

    // Method to disable all player scripts
    void DisablePlayerScripts()
    {
        if (playerScripts != null)
        {
            foreach (MonoBehaviour script in playerScripts)
            {
                if (script != this) // Ensure we don't disable the PauseMenu itself
                {
                    script.enabled = false;
                }
            }
        }
    }

    // Method to enable all player scripts
    void EnablePlayerScripts()
    {
        if (playerScripts != null)
        {
            foreach (MonoBehaviour script in playerScripts)
            {
                script.enabled = true;
            }
        }
    }
}
