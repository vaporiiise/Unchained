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
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

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
        playerRB.MovePosition(playerRB.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);

        if (isDodging)
        {
            playerRB.MovePosition(playerRB.position + dodgeDirection * dodgeSpeed * Time.fixedDeltaTime);

            if (Time.time > lastDodgeTime + dodgeDuration)
            {
                isDodging = false;

                isInvincible = false;
                playerCol.enabled = true;
            }
        }
    }
}