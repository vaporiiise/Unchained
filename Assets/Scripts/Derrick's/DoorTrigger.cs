using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform box1; // First box
    public Transform box2; // Second box
    public Transform targetPosition1; // Target position for box 1
    public Transform targetPosition2; // Target position for box 2
    public GameObject door; // The door object (box)
    public SpriteRenderer lockSprite; // Sprite that disappears when unlocked
    public float threshold = 0.5f; // Distance threshold for activation
    public AudioClip doorUnlockSound; // Sound effect when unlocked
    public MonoBehaviour scriptToDisable; // Script to disable after unlock
    public Transform moveToPosition; // Where the door moves
    public float moveSpeed = 20f; // Speed at which the door moves
    public Camera mainCamera; // Reference to the camera
    public Transform cameraTarget; // The position the camera should pan to
    public float cameraPanSpeed = 5f; // Speed of the camera pan
    public float shakeDuration = 0.2f; // How long the camera shakes
    public float shakeIntensity = 0.2f; // How strong the shake is
    public float spriteFadeSpeed = 1.5f; // Speed at which sprite fades out

    private bool doorOpened = false;

    void Start()
    {
        if (lockSprite != null)
        {
            Color c = lockSprite.color;
            c.a = 1; // Make sure the sprite is fully visible at the start
            lockSprite.color = c;
        }
    }

    void Update()
    {
        if (Vector2.Distance(box1.position, targetPosition1.position) < threshold &&
            Vector2.Distance(box2.position, targetPosition2.position) < threshold && !doorOpened)
        {
            doorOpened = true;
            StartCoroutine(OpenDoorSequence());
        }
    }

    IEnumerator OpenDoorSequence()
    {
        // Disable the specified script
        if (scriptToDisable != null)
        {
            scriptToDisable.enabled = false;
        }

        // Play the unlock sound
        if (doorUnlockSound != null)
        {
            AudioSource.PlayClipAtPoint(doorUnlockSound, Camera.main.transform.position, 1.0f);
        }

        // Camera shake effect
        yield return StartCoroutine(CameraShake());

        // Start fading out the lock sprite
        if (lockSprite != null)
        {
            StartCoroutine(FadeOutSprite());
        }

        // Move the door really fast
        float startTime = Time.time;
        Vector3 startPosition = door.transform.position;
        Vector3 endPosition = moveToPosition.position;
        float journeyLength = Vector3.Distance(startPosition, endPosition);

        while (Vector3.Distance(door.transform.position, endPosition) > 0.05f)
        {
            float elapsedTime = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = elapsedTime / journeyLength;
            door.transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
            yield return null;
        }

        door.transform.position = endPosition; // Ensure the door reaches its position

        // Pan the camera to the door position
        yield return StartCoroutine(PanCamera());

        // Wait 0.5 seconds before reactivating the script
        yield return new WaitForSeconds(0.5f);

        // Reactivate the previously disabled script
        if (scriptToDisable != null)
        {
            scriptToDisable.enabled = true;
        }
    }

    IEnumerator CameraShake()
    {
        Vector3 originalPos = mainCamera.transform.position;
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            mainCamera.transform.position = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalPos; // Reset position
    }

    IEnumerator FadeOutSprite()
    {
        Color c = lockSprite.color;
        float alpha = 1f;
        float startTime = Time.time;

        while (alpha > 0f) // Fully fade out
        {
            alpha = Mathf.Clamp01(1f - (Time.time - startTime) * spriteFadeSpeed);
            c.a = alpha;
            lockSprite.color = c;
            yield return null;
        }

        c.a = 0;
        lockSprite.color = c; // Ensure it's completely invisible
    }

    IEnumerator PanCamera()
    {
        Vector3 startCamPos = mainCamera.transform.position;
        Vector3 endCamPos = cameraTarget.position;
        float startTime = Time.time;
        float duration = 0.5f; // Camera pan duration

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            mainCamera.transform.position = Vector3.Lerp(startCamPos, endCamPos, t);
            yield return null;
        }

        mainCamera.transform.position = endCamPos; // Ensure final position is correct

        yield return new WaitForSeconds(0.5f); // Keep camera on the door for 0.5 seconds
    }
}