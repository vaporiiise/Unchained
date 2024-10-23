using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 5F;
    private Collider2D playerCol;
    private Rigidbody2D playerRB;

    private Vector2 movement;
    public float dodgeSpeed = 20F;
    public float dodgeDuration = 0.2F;
    public float dodgeCooldown = 1F;
    public bool isInvincible = false;

    private float lastDodgeTime;
    private bool isDodging = false;
    private Vector2 dodgeDirection;

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // Check if the game is paused
        if (PauseMenu.GameIsPaused)
        {
            return; // Exit if paused
        }

        // Get player movement input only when the game is not paused
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Handle dodge action when the game is not paused
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastDodgeTime + dodgeCooldown)
        {
            isDodging = true;
            lastDodgeTime = Time.time;
            dodgeDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

            isInvincible = true;
            playerCol.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        // Move the player only when the game is not paused
        if (PauseMenu.GameIsPaused)
        {
            return; // Exit if paused
        }

        // Move the player based on input
        playerRB.MovePosition(playerRB.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        // Handle dodging
        if (isDodging)
        {
            playerRB.MovePosition(playerRB.position + dodgeDirection * dodgeSpeed * Time.fixedDeltaTime);

            // Reset dodge state after duration
            if (Time.time > lastDodgeTime + dodgeDuration)
            {
                isDodging = false;
                isInvincible = false;
                playerCol.enabled = true;
            }
        }
    }
}