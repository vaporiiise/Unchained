using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleScFadeOut : MonoBehaviour
{
    public RawImage rawImage;                 
    public TextMeshProUGUI uiText;            
    public float delayBeforeFadeIn = 1f;      
    public float fadeInDuration = 1f;         
    public float fadeOutDuration = 1f;        
    public AudioSource audioSource;           
    public AudioClip anyKey;                  
    public Vector3 cameraTargetPosition;      
    public float cameraMoveDuration = 2f;     
    public float smoothTime = 0.3f;           

    public RectTransform buttonToMove;        
    public float buttonTargetXPos;            
    public float buttonSmoothTime = 0.2f;     

    public RectTransform secondButton;        
    public float secondButtonTargetXPos;      
    public float secondButtonSmoothTime = 0.2f; 

    public RectTransform thirdButton;         
    public float thirdButtonTargetXPos;       
    public float thirdButtonSmoothTime = 0.2f; 

    public RectTransform fourthButton;        
    public float fourthButtonTargetXPos;      
    public float fourthButtonSmoothTime = 0.2f; 

    public RectTransform fifthButton;         
    public float fifthButtonTargetXPos;       
    public float fifthButtonSmoothTime = 0.2f; 
    public float delayBetweenButtonMoves = 0.2f; 

    private bool isFadingOut = false;         
    private bool fadeInComplete = false;      

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

        yield return MoveButtonToTarget(buttonToMove, buttonTargetXPos, buttonSmoothTime);
        yield return MoveButtonToTarget(secondButton, secondButtonTargetXPos, secondButtonSmoothTime);
        yield return MoveButtonToTarget(thirdButton, thirdButtonTargetXPos, thirdButtonSmoothTime);
        yield return MoveButtonToTarget(fourthButton, fourthButtonTargetXPos, fourthButtonSmoothTime);
        yield return MoveButtonToTarget(fifthButton, fifthButtonTargetXPos, fifthButtonSmoothTime);
    }

    private IEnumerator MoveButtonToTarget(RectTransform button, float targetXPos, float smoothTime)
    {
        Vector3 velocity = Vector3.zero;
        Vector3 startPos = button.anchoredPosition;
        Vector3 targetPos = new Vector3(targetXPos, startPos.y, startPos.z);

        while (Vector3.Distance(button.anchoredPosition, targetPos) > 0.1f)
        {
            button.anchoredPosition = Vector3.SmoothDamp(button.anchoredPosition, targetPos, ref velocity, smoothTime);
            yield return null;
        }

        // Ensure the button reaches the exact target position
        button.anchoredPosition = targetPos;
    }
}