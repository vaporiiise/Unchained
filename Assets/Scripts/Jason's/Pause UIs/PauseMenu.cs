using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused { get; private set; }

    public GameObject pauseMenuUI; // Pause menu UI
    public GameObject settingsPanel; // Settings panel UI
    public GameObject exitPanel; // Exit confirmation panel UI

    public GameObject player; // Player GameObject

    [SerializeField]
    private KeyCode pauseKey = KeyCode.Escape; // Key to toggle pause

    private MonoBehaviour[] playerScripts; // Store player scripts for enabling/disabling

    void Start()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (exitPanel != null) exitPanel.SetActive(false);

        if (player != null)
        {
            playerScripts = player.GetComponents<MonoBehaviour>();
        }
    }

    void Update()
    {
        if (pauseMenuUI != null && Input.GetKeyDown(pauseKey))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public static void TogglePause()
    {
        GameIsPaused = !GameIsPaused;
        Time.timeScale = GameIsPaused ? 0f : 1f;
    }

    void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (exitPanel != null) exitPanel.SetActive(false);

        Time.timeScale = 1f;
        GameIsPaused = false;
        EnablePlayerScripts();
    }

    void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        GameIsPaused = true;
        DisablePlayerScripts();
    }

    public void OpenSettingsPanel()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (exitPanel != null) exitPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
    }

    // **🔹 New Exit Panel Functions**
    public void OpenExitPanel()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (exitPanel != null) exitPanel.SetActive(true);
    }

    public void CloseExitPanel()
    {
        if (exitPanel != null) exitPanel.SetActive(false);
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
    }

    public void ConfirmExit()
    {
        Debug.Log("Exiting Game...");
        Application.Quit(); // Quits the game (only works in a built application)
    }

    void DisablePlayerScripts()
    {
        if (playerScripts != null)
        {
            foreach (MonoBehaviour script in playerScripts)
            {
                if (script != this)
                {
                    script.enabled = false;
                }
            }
        }
    }

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
