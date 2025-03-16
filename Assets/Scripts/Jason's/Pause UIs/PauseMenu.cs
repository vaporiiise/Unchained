using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused { get; private set; }

    public GameObject pauseCanvas; // Reference to the pause menu canvas
    public GameObject player; // Reference to the player GameObject

    [SerializeField]
    private KeyCode pauseKey = KeyCode.Escape; // Configurable key for pausing the game

    [Header("Scripts to Disable")]
    public MonoBehaviour[] scriptsToDisable; // Array of scripts to disable when paused

    private Rigidbody2D playerRigidbody;
    private Animator playerAnimator;

    void Start()
    {
        // Ensure the pause canvas is hidden at the start, if assigned
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }

        // Get components, if assigned
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody2D>();
            playerAnimator = player.GetComponent<Animator>();

            // Auto-detect Tilemovement if no scripts are assigned
            if (scriptsToDisable.Length == 0)
            {
                scriptsToDisable = new MonoBehaviour[] { player.GetComponent<Tilemovement>() };
            }
        }
    }

    void Update()
    {
        if (pauseCanvas != null && Input.GetKeyDown(pauseKey))
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

        // Toggle all scripts in the array
        ToggleScripts(!GameIsPaused);
    }

    void Resume()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(false);
        }

        Time.timeScale = 1f;
        GameIsPaused = false;

        EnablePlayerComponents();
    }

    void Pause()
    {
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(true);
        }

        Time.timeScale = 0f;
        GameIsPaused = true;

        DisablePlayerComponents();
    }

    void ToggleScripts(bool enable)
    {
        foreach (MonoBehaviour script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = enable;
            }
        }
    }

    void DisablePlayerComponents()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector2.zero; // Stop movement
            playerRigidbody.simulated = false; // Disable physics
        }

        if (playerAnimator != null)
        {
            playerAnimator.enabled = false; // Stop animations
        }
    }

    void EnablePlayerComponents()
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.simulated = true; // Re-enable physics
        }

        if (playerAnimator != null)
        {
            playerAnimator.enabled = true; // Resume animations
        }
    }
}
    