using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerHallsPlayerAnimator : MonoBehaviour
{
    private KeyCode lastKeyPressed;
    public float cD = 0.3f; // Cooldown between attacks
    public float inputWindowTime = 1f; // Time window for combo input

    public Animator anim;
    public bool isWalkingDown = false;
    public bool isWalkingLeft = false;
    public bool isWalkingRight = false;
    public bool isWalkingUp = false;

    private bool isAttacking = false; // Flag to indicate whether the player is currently attacking
    private int comboStep = 0; // Current combo step
    private bool canContinueCombo = false; // Determines if the player can continue the combo

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking) return; // Prevent movement while attacking

        // Movement Handling
        HandleMovement();

        // Attack Combo Handling
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Detect Mouse0 for initiating or continuing combo
        {
            if (comboStep == 0) // If no combo in progress, start a new combo
            {
                StartCoroutine(HandleCombo());
            }
            else if (canContinueCombo) // If combo in progress and within input window
            {
                ContinueCombo();
            }
        }
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.S))
        {
            isWalkingDown = true;
            anim.Play("WalkDown");
        }
        else if (Input.GetKey(KeyCode.A))
        {
            isWalkingLeft = true;
            anim.Play("WalkLeft");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            isWalkingRight = true;
            anim.Play("WalkRight");
        }
        else if (Input.GetKey(KeyCode.W))
        {
            isWalkingUp = true;
            anim.Play("WalkUp");
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            anim.Play("UpIdle");
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.Play("LeftIdle");
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            anim.Play("Idle");
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.Play("RightIdle");
        }
    }

    // Coroutine to handle the combo animation sequence
    IEnumerator HandleCombo()
    {
        isAttacking = true;
        comboStep = 1; // Start with the first step
        anim.Play("PlayerCombo1");
        canContinueCombo = true;

        // Wait for animation to complete + cooldown duration
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + cD);

        // Start a timer for the combo input window
        yield return new WaitForSeconds(inputWindowTime);

        canContinueCombo = false; // Combo input window has ended
        ResetCombo(); // Reset combo if not continued
    }

    // Method to continue the combo if input is received within the time window
    private void ContinueCombo()
    {
        if (comboStep < 4) // Limit to 4 combo steps
        {
            comboStep++;
            anim.Play("PlayerCombo" + comboStep); // Trigger next combo animation

            if (comboStep == 4)
            {
                // If max combo is reached, finish combo sequence
                StartCoroutine(FinishCombo());
            }
        }
    }

    // Coroutine to finish the combo and reset after the final animation
    IEnumerator FinishCombo()
    {
        // Wait for the last animation to finish + cooldown
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + cD);

        ResetCombo(); // Reset combo after final animation ends
    }

    // Resets the combo step and related flags
    private void ResetCombo()
    {
        comboStep = 0;
        isAttacking = false;
        anim.Play("Idle"); // Return to idle state
    }
} 