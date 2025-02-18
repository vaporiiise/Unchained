using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DoorUnlockEffect : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Assign the player
    public Transform objectToMove; // Object that moves
    public float moveYDistance = 2f; // How much the object moves in Y
    public GameObject fadeOutObject; // The object to fade out (sprite)
    public float fadeDuration = 1f; // Fade-out duration

    
    [Header("Effects")]
    public ParticleSystem effectPrefab; // The particle effect
    public Transform effectSpawnPoint; // Where to spawn the particle effect
    public AudioSource soundEffect; // Sound effect to play

    [Header("Screen Shake Settings")]
    public float shakeDuration = 0.2f; // How long the shake lasts
    public float shakeIntensity = 0.1f; // How intense the shake is

    [Header("Trigger Settings")]
    public Vector2 triggerAreaCenter; // Center of the trigger area
    public float triggerRadius = 2f; // Radius of the trigger area

    private bool effectTriggered = false;

    private void Update()
    {
        if (effectTriggered) return;

        // Check if player is inside the area
        if (Vector2.Distance(player.position, triggerAreaCenter) <= triggerRadius)
        {
            effectTriggered = true;
            StartEffect();
        }
    }

    void StartEffect()
    {
        // Move the object upwards (only Y movement)
        objectToMove.position += new Vector3(0, moveYDistance, 0);

        // Start fade-out effect
        StartCoroutine(FadeOutObject());

        // Play sound effect
        if (soundEffect != null)
        {
            soundEffect.Play();
        }

        // Spawn and play the particle effect at a custom position
        if (effectPrefab != null && effectSpawnPoint != null)
        {
            ParticleSystem newEffect = Instantiate(effectPrefab, effectSpawnPoint.position, Quaternion.identity);
            newEffect.Play();
            
            // Start screen shake when particle effect plays
            StartCoroutine(ScreenShake());
        }
    }

    IEnumerator FadeOutObject()
    {
        SpriteRenderer sr = fadeOutObject.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float startAlpha = sr.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
            yield return null;
        }

        fadeOutObject.SetActive(false);
    }

    IEnumerator ScreenShake()
    {
        Camera mainCam = Camera.main; 
        if (mainCam == null) yield break; // Ensure we have a camera

        Vector3 originalPos = mainCam.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);
            mainCam.transform.position = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        mainCam.transform.position = originalPos; // Reset camera position
    }

    private void OnDrawGizmos()
    {
        // Draw a circle in the Scene view to show the trigger area
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(triggerAreaCenter, triggerRadius);

        // Draw an arrow showing the object’s movement direction
        if (objectToMove != null)
        {
            Gizmos.color = Color.blue;
            Vector3 arrowStart = objectToMove.position;
            Vector3 arrowEnd = arrowStart + new Vector3(0, moveYDistance, 0);
            Gizmos.DrawLine(arrowStart, arrowEnd);
            Gizmos.DrawSphere(arrowEnd, 0.2f);
        }

        // Draw particle effect spawn position
        if (effectSpawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(effectSpawnPoint.position, 0.2f);
        }
    }
}