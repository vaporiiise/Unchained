using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTriggerByPosition : MonoBehaviour
{
    public Vector2 triggerPosition; // The position where the reset can be triggered
    public float activationRadius = 1f; // The radius around the position for the player to be in

    public Transform playerTransform; // Reference to the player's transform

    private void Start()
    {
        // Find the player GameObject in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Please ensure the player GameObject has the 'Player' tag.");
        }
    }

    private void Update()
    {
        // Check if the player is within the activation radius of the specified position
        if (playerTransform != null && Vector2.Distance(playerTransform.position, triggerPosition) <= activationRadius)
        {
            // Check if the 'F' key is pressed
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Trigger the global reset, but keep the player's position unchanged
                GlobalResetManager.Instance.ResetAllExceptPlayer();
                Debug.Log("Reset triggered by player at position: " + playerTransform.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw a sphere in the editor to visualize the activation radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(triggerPosition, activationRadius);
    }
}