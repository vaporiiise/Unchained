using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MoveTextOnButtonClick : MonoBehaviour
{
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
    }

    void StartMovement()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveObject());
        }
    }

    System.Collections.IEnumerator MoveObject()
    {
        isMoving = true;
        
        Vector3 objectStartPosition = targetObject.transform.position;
        Vector3 objectTargetPos = objectTargetPosition.position;

        float elapsedTime = 0.0f;

        // Calculate the duration based on distance and speed
        float maxDistance = Mathf.Max(
            Vector3.Distance(objectStartPosition, objectTargetPos)
        );
        float duration = maxDistance / moveSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Smoothly move the camera

            // Smoothly move the object
            targetObject.transform.position = Vector3.Lerp(objectStartPosition, objectTargetPos, elapsedTime / duration);

            yield return null; // Wait for the next frame
        }

        // Ensure the final positions are exact
        targetObject.transform.position = objectTargetPos;

        isMoving = false;
    }
}
