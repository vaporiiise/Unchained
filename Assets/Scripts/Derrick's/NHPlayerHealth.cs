using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NHPlayerHealth : MonoBehaviour
{
    public int maxHealth = 150;
    private int currentHealth;
    public AudioClip takeDamageSound;
    private AudioSource audioSource;
    public AudioClip healSound;

    public List<Image> healthImages; 
    public GameObject damageIndicator;    
    public float damageDisplayDuration = 0.5f; 

    public GameObject deathCanvas;        // Canvas to activate on death
    public List<GameObject> otherCanvases; // List of other canvases to disable on death

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    
        // Check if the saved health exists, and if not, initialize it to maxHealth
        if (GameManager.Instance.savedPlayerHealth > 0)
        {
            currentHealth = GameManager.Instance.savedPlayerHealth; 
        }
        else 
        {
            currentHealth = maxHealth;  // If no saved health or it's 0, set to max health
        }

        // Ensure the health is saved in GameManager after initialization
        GameManager.Instance.SavePlayerHealth(currentHealth);

        UpdateHealthUI();  // Update the UI with the current health

        if (damageIndicator != null)
        {
            damageIndicator.gameObject.SetActive(false);  // Ensure damage indicator is off at the start
        }

        if (deathCanvas != null)
        {
            deathCanvas.SetActive(false);  // Ensure death canvas is off at the start
        }
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.JoystickButton5)))
        {
           TakeDamage(10);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        UpdateHealthUI();

        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);
        audioSource.PlayOneShot(takeDamageSound);
        GameManager.Instance.SavePlayerHealth(currentHealth);

        if (damageIndicator != null)
        {
            StartCoroutine(ShowDamageIndicator());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ShowDamageIndicator()
    {
        damageIndicator.gameObject.SetActive(true); 
        yield return new WaitForSeconds(damageDisplayDuration); 
        damageIndicator.gameObject.SetActive(false); 
    }

    void UpdateHealthUI()
    {
        // Calculate how many health images should be visible based on current health.
        int remainingSprites = Mathf.CeilToInt((float)currentHealth / 10); // Each sprite represents 10 health

        // Ensure the number of visible sprites does not exceed the size of healthImages
        remainingSprites = Mathf.Clamp(remainingSprites, 0, healthImages.Count);

        // Update the health images based on the remaining health
        for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < remainingSprites)
            {
                healthImages[i].gameObject.SetActive(true);  // Enable health image
            }
            else
            {
                healthImages[i].gameObject.SetActive(false);  // Disable health image
            }
        }
    }

    void Die()
    {
        Debug.Log("Player is dead!");

        AudioListener.pause = true;

        Time.timeScale = 0;

        if (deathCanvas != null)
        {
            deathCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Death canvas is not assigned in the inspector!");
        }

        // Disable other canvases
        foreach (GameObject canvas in otherCanvases)
        {
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
        
    }

    void HealPlayer(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);  // Heal player but don't exceed max health
        Debug.Log("Player Healed: " + currentHealth);
        UpdateHealthUI();
    }
}