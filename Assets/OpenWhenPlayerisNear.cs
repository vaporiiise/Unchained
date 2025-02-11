using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenWhenPlayerisNear : MonoBehaviour
{
    public string targetTag = "BossRoom";  // Tag of the first object
    public Transform player;  // Reference to the player
    public float threshold = 0.5f;  // Distance threshold
    public Animator animator; // Reference to the Animator

    private bool isNear = false;

    void Update()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
        bool playerClose = false;

        foreach (GameObject target in targets)
        {
            if (Vector3.Distance(target.transform.position, player.position) <= threshold)
            {
                playerClose = true;
                break;
            }
        }

        if (playerClose != isNear) // Only update if state changes
        {
            isNear = playerClose;
            animator.SetBool("isNear", isNear);
        }
    }
}
