using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnHold : MonoBehaviour
{
    public Animator animator;  // Assign your Animator in the Inspector
    private bool isHoldingEsc = false;
    private float animationTime = 0f;
    private float idleTimer = 0f;
    public float idleTimeout = 71f; // Time before auto scene change

    void Update()
    {
        idleTimer += Time.deltaTime; // Track idle time

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isHoldingEsc = true;
            idleTimer = 0f; // Reset idle timer on input
            animator.Play("HOLDCIRCLE", 0, 0); // Start animation from beginning
            animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isHoldingEsc = false;
            animator.Play("IdleCircle", 0, 0); // Reset animation (optional)
        }

        if (isHoldingEsc && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            LoadNextScene();
        }

        if (idleTimer >= idleTimeout) // If no input for 71 seconds
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Main Menu NEW"); // Replace with your actual scene name
    }
}
