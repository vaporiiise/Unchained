using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScFadeOut : MonoBehaviour
{
    public RawImage rawImage;             // Reference to the RawImage component
    public TextMeshProUGUI uiText;        // Reference to the TextMeshProUGUI component
    public float fadeDuration = 1f;       // Duration of the fade-out
    private bool isFading = false; // To ensure fade out only happens once
    public AudioClip anyKey;
    public AudioSource audioSource;
    public Vector3 cameraTargetPosition;  // Target position for the camera
    public float cameraMoveDuration = 2f;
    private void Start()
    {
        anyKey = GetComponent<AudioSource>().clip;
    }

    private void Update()
    {
        // Start fading when any key is pressed and fade hasn't started yet
        if (Input.anyKeyDown && !isFading)
        {
            StartCoroutine(FadeOutElements());
            isFading = true;
            audioSource.PlayOneShot(anyKey);
        }
    }

    private IEnumerator FadeOutElements()
    {
        float elapsedTime = 0f;
        Color imageColor = rawImage.color;
        Color textColor = uiText.color;

        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            
            // Update alpha for both image and text
            imageColor.a = alpha;
            rawImage.color = imageColor;
            
            textColor.a = alpha;
            uiText.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is set to 0 (fully transparent)
        imageColor.a = 0f;
        rawImage.color = imageColor;
        
        textColor.a = 0f;
        uiText.color = textColor;
        StartCoroutine(MoveCameraToTarget());
    }
    
    private IEnumerator MoveCameraToTarget()
    {
        Camera mainCamera = Camera.main;
        Vector3 startPosition = mainCamera.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < cameraMoveDuration)
        {
            mainCamera.transform.position = Vector3.Lerp(startPosition, cameraTargetPosition, elapsedTime / cameraMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera reaches the exact target position
        mainCamera.transform.position = cameraTargetPosition;
    }

}
