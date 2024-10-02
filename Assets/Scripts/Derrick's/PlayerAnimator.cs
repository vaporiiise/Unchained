using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator anim; // Reference to the Animator component

    void Start()
    {
        // Get the Animator component attached to the player
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Check for input and set the corresponding animation parameters
        if (Input.GetKey(KeyCode.W)) // Up movement
        {
            anim.SetBool("moveUp", true); // Set moveUp parameter to true
        }
        else
        {
            anim.SetBool("moveUp", false); // Set moveUp parameter to false if W is not pressed
        }

        if (Input.GetKey(KeyCode.A)) // Left movement
        {
            anim.SetBool("moveLeft", true); // Set moveLeft parameter to true
        }
        else
        {
            anim.SetBool("moveLeft", false); // Set moveLeft parameter to false if A is not pressed
        }

        if (Input.GetKey(KeyCode.S)) // Down movement
        {
            anim.SetBool("moveDown", true); // Set moveDown parameter to true
        }
        else
        {
            anim.SetBool("moveDown", false); // Set moveDown parameter to false if S is not pressed
        }

        if (Input.GetKey(KeyCode.D)) // Right movement
        {
            anim.SetBool("moveRight", true); // Set moveRight parameter to true
        }
        else
        {
            anim.SetBool("moveRight", false); // Set moveRight parameter to false if D is not pressed
        }
    }
}