using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMoveOnClick : MonoBehaviour
{
    public Camera targetCamera; // The camera to move
    public Vector3 cameraTargetPosition; // Target position for the camera
    public GameObject targetObject; // The object to move
    public Transform objectTargetPosition; 
    public float moveSpeed = 5.0f; // Speed of movement
    public Button triggerButton;

    private bool isMoving = false; // Track if movement is ongoing

    void Start()
    {
        if (triggerButton != null)
        {
            triggerButton.onClick.AddListener(StartMovement);
        }

        if (targetCamera == null)
        {
            targetCamera = Camera.main; // Default to main camera if not set
        }
    }

    void StartMovement()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveCameraAndObject());
        }
    }

    System.Collections.IEnumerator MoveCameraAndObject()
    {
        isMoving = true;

        Vector3 cameraStartPosition = targetCamera.transform.position;
        Vector3 objectStartPosition = targetObject.transform.position;
        Vector3 objectTargetPos = objectTargetPosition.position;

        float elapsedTime = 0.0f;

        // Calculate the duration based on distance and speed
        float maxDistance = Mathf.Max(
            Vector3.Distance(cameraStartPosition, cameraTargetPosition),
            Vector3.Distance(objectStartPosition, objectTargetPos)
        );
        float duration = moveSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Smoothly move the camera
            targetCamera.transform.position = Vector3.Lerp(cameraStartPosition, cameraTargetPosition, elapsedTime / duration);

            // Smoothly move the object
            targetObject.transform.position = Vector3.Lerp(objectStartPosition, objectTargetPos, elapsedTime / duration);

            yield return null; // Wait for the next frame
        }

        // Ensure the final positions are exact
        targetCamera.transform.position = cameraTargetPosition;
        targetObject.transform.position = objectTargetPos;

        isMoving = false;
    }
}