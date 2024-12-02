using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NHPlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public AudioClip takeDamageSound;
    private AudioSource audioSource;

    public List<Image> healthImages; 
    public GameObject damageIndicator;    
    public float damageDisplayDuration = 0.5f; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (damageIndicator != null)
        {
            damageIndicator.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Heal player when pressing Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            HealPlayer(20);
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
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);
        UpdateHealthUI();
        audioSource.PlayOneShot(takeDamageSound);

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
        int remainingSprites = Mathf.CeilToInt((float)currentHealth / (maxHealth / healthImages.Count));

        for (int i = 0; i < healthImages.Count; i++)
        {
            if (i < remainingSprites)
            {
                healthImages[i].enabled = true; 
            }
            else
            {
                healthImages[i].enabled = false; 
            }
        }
    }

    void Die()
    {
        Debug.Log("Player is dead!");
        SceneManager.LoadScene(1);
    }

    void HealPlayer(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);  // Heal player but don't exceed max health
        Debug.Log("Player Healed: " + currentHealth);
        UpdateHealthUI();
    }
}
