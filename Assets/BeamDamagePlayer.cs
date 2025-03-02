using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamDamagePlayer : MonoBehaviour
{
    [Header("Beam Damage")]
    public int maxHealth = 30;
    private int currentHealth;
    
    [Header("Screen Shake Settings")]
    public float shakeDuration = 0.2f; 
    public float shakeIntensity = 0.1f;
    
    [Header("Damage Settings")]
    public AudioSource soundEffect;
    public GameObject damageIndicator;
    public float damageDisplayDuration = 0.5f; 




    private void Start()
    {
        currentHealth = maxHealth;
        Debug.Log(currentHealth);
        
        if (damageIndicator != null)
        {
            damageIndicator.gameObject.SetActive(false);  // Ensure damage indicator is off at the start
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TakeDamage();
            Debug.Log(currentHealth);
            StartCoroutine(ScreenShake());
            PlaySound();
        }
        
    }

    private void TakeDamage()
    {
        currentHealth -= 10;
        
        if (damageIndicator != null)
        {
            StartCoroutine(ShowDamageIndicator());
        }
    }
    IEnumerator ScreenShake()
    {
        Camera mainCam = Camera.main; 
        if (mainCam == null) yield break; 

        Vector3 originalPos = mainCam.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float offsetX = UnityEngine.Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = UnityEngine.Random.Range(-shakeIntensity, shakeIntensity);
            mainCam.transform.position = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = originalPos; 
    }

    private void PlaySound()
    {
        soundEffect.Play();
    }
    
    IEnumerator ShowDamageIndicator()
    {
        damageIndicator.gameObject.SetActive(true); 
        yield return new WaitForSeconds(damageDisplayDuration); 
        damageIndicator.gameObject.SetActive(false); 
    }
}
