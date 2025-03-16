using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class bossHealth : MonoBehaviour
{
    
    public int maxHealth = 200;
    public int currentHealth;
    public HealthBar healthBar;
    public int sceneInt = 3;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        healthBar.SetHealth(currentHealth);
        
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(sceneInt);
        }
    }
}
