using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendPlayerBack : MonoBehaviour
{
    public GameObject player;       // Assign the player GameObject in the inspector
    public float targetZPosition;   // The target Z position
    public Vector3 requiredPosition;  // The position where the key press will work
    public float positionTolerance = 0.1f;  // How close the player needs to be to the required position

    void Update()
    {
        // Check if the player is within a certain range of the required position
        if (Vector3.Distance(player.transform.position, requiredPosition) <= positionTolerance)
        {
            // Check if the "F" key is pressed
            if (Input.GetKeyDown(KeyCode.F))
            {
                // Move the player to the target Z position
                Vector3 currentPosition = player.transform.position;
                player.transform.position = new Vector3(currentPosition.x, currentPosition.y, targetZPosition);
            }
        }
    }

    // Draw gizmos in the Scene view to visualize the required position and tolerance area
    void OnDrawGizmos()
    {
        // Set the gizmo color to green
        Gizmos.color = Color.green;
        
        // Draw a sphere at the required position
        Gizmos.DrawSphere(requiredPosition, 0.1f);
        
        // Set the gizmo color to red (for the tolerance area)
        Gizmos.color = Color.red;
        
        // Draw a wireframe sphere around the required position with the given tolerance
        Gizmos.DrawWireSphere(requiredPosition, positionTolerance);
    }
}
