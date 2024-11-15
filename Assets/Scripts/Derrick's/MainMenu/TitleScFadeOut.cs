using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScFadeOut : MonoBehaviour
{
    public RawImage rawImage;                 // Reference to the RawImage component
    public TextMeshProUGUI uiText;            // Reference to the TextMeshProUGUI component
    public float delayBeforeFadeIn = 1f;      // Delay before the fade-in starts
    public float fadeInDuration = 1f;         // Duration of the fade-in
    public float fadeOutDuration = 1f;        // Duration of the fade-out
    public AudioSource audioSource;           // Reference to the AudioSource component
    public AudioClip anyKey;                  // Sound clip to play when any key is pressed
    public Vector3 cameraTargetPosition;      // Target position for the camera
    public float cameraMoveDuration = 2f;     // Duration of the camera movement
    public float smoothTime = 0.3f;           // Smooth time for SmoothDamp

    public RectTransform buttonToMove;        // Reference to the UI Button's RectTransform
    public float buttonTargetXPos;            // Target X position for the button
    public float buttonSmoothTime = 0.2f;     // Smooth time for button movement

    private bool isFadingOut = false;         // To ensure fade-out only happens once
    private bool fadeInComplete = false;      // To track if fade-in has finished

    private void Start()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            anyKey = audioSource.clip;
        }

        StartCoroutine(FadeInText());
    }

    private void Update()
    {
        // After fade-in is complete, wait for any key to start fade-out
        if (fadeInComplete && Input.anyKeyDown && !isFadingOut)
        {
            StartCoroutine(FadeOutElements());
            isFadingOut = true;
            audioSource?.PlayOneShot(anyKey);
        }
    }

    private IEnumerator FadeInText()
    {
        // Wait for the specified delay before starting the fade-in
        yield return new WaitForSeconds(delayBeforeFadeIn);

        float elapsedTime = 0f;
        Color textColor = uiText.color;
        textColor.a = 0f;  // Start with the text fully transparent
        uiText.color = textColor;

        while (elapsedTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            textColor.a = alpha;
            uiText.color = textColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is set to 1 (fully opaque)
        textColor.a = 1f;
        uiText.color = textColor;
        fadeInComplete = true; // Mark fade-in as complete
    }

    private IEnumerator FadeOutElements()
    {
        float elapsedTime = 0f;
        Color imageColor = rawImage.color;
        Color textColor = uiText.color;

        while (elapsedTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);

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

        // Start the camera movement coroutine after the fade-out completes
        StartCoroutine(MoveCameraToTarget());
    }

    private IEnumerator MoveCameraToTarget()
    {
        Camera mainCamera = Camera.main;
        Vector3 velocity = Vector3.zero;  // Required by SmoothDamp to smoothly reach the target position
        float elapsedTime = 0f;

        // Smoothly move the camera to the target position using SmoothDamp
        while (elapsedTime < cameraMoveDuration)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, cameraTargetPosition, ref velocity, smoothTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the camera reaches the exact target position
        mainCamera.transform.position = cameraTargetPosition;

        // After camera movement is done, move the button to the target position
        StartCoroutine(MoveButtonToTarget());
    }

    private IEnumerator MoveButtonToTarget()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 startPos = buttonToMove.anchoredPosition;
        Vector3 targetPos = new Vector3(buttonTargetXPos, startPos.y, startPos.z);

        while (Vector3.Distance(buttonToMove.anchoredPosition, targetPos) > 0.1f)
        {
            buttonToMove.anchoredPosition = Vector3.SmoothDamp(buttonToMove.anchoredPosition, targetPos, ref velocity, buttonSmoothTime);
            yield return null;
        }

        // Ensure the button reaches the exact target position
        buttonToMove.anchoredPosition = targetPos;
    }
}
