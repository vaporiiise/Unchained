using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SendPlayerBack : MonoBehaviour
{
    public GameObject player;         // Reference to the player
    public float targetZPosition;     // Target Z position for the player
    public Vector3 requiredPosition;  // Required position to trigger the action
    public float positionTolerance = 0.1f; // Tolerance for required position

    public GameObject objectToMove1;  // First object to move
    public Vector3 moveTargetPosition1; // Target position for the first object
    public float moveDuration1 = 1.5f; // Duration for the first object's movement

    public GameObject objectToMove2;  // Second object to move
    public Vector3 moveTargetPosition2; // Target position for the second object
    public float moveDuration2 = 1.5f; // Duration for the second object's movement

    private bool isMoving = false; // Prevents multiple overlapping movements

    void Update()
    {
        if (Vector3.Distance(player.transform.position, requiredPosition) <= positionTolerance)
        {
            if (Input.GetKeyDown(KeyCode.T) && !isMoving)
            {
                Vector3 currentPosition = player.transform.position;
                player.transform.position = new Vector3(currentPosition.x, currentPosition.y, targetZPosition);

                // Start moving both objects
                StartCoroutine(MoveObjectToPosition(objectToMove1, moveTargetPosition1, moveDuration1));
                StartCoroutine(MoveObjectToPosition(objectToMove2, moveTargetPosition2, moveDuration2));
            }
        }
    }

    private IEnumerator MoveObjectToPosition(GameObject obj, Vector3 targetPosition, float duration)
    {
        isMoving = true; // Prevent overlapping movements

        Vector3 startPosition = obj.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            t = t * t * (3f - 2f * t); // Smoothstep interpolation for smooth movement
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        obj.transform.position = targetPosition; // Ensure exact final position
        isMoving = false; // Allow new movements
    }

    void OnDrawGizmos()
    {
        // Draw the required position as a sphere
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(requiredPosition, 0.1f);

        // Draw the tolerance as a wireframe sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(requiredPosition, positionTolerance);

        // Draw a line to the move target positions
        if (objectToMove1 != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(objectToMove1.transform.position, moveTargetPosition1);
        }

        if (objectToMove2 != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(objectToMove2.transform.position, moveTargetPosition2);
        }
    }
}