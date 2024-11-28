using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NHPlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public List<Image> healthImages; // List of UI Images (assign in Inspector)

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
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
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure health doesn't drop below 0
        Debug.Log("Player Health: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        int remainingSprites = Mathf.CeilToInt((float)currentHealth / (maxHealth / healthImages.Count));

        for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < remainingSprites)
            {
                healthImages[i].enabled = true; // Show sprite
            }
            else
            {
                healthImages[i].enabled = false; // Hide sprite
            }
        }
    }

    void Die()
    {
        Debug.Log("Player is dead!");
        // Add death logic here (e.g., restart level, show game over screen)
    }
    
    
}
