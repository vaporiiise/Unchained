using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTVMovement : MonoBehaviour
{
    public Transform player;
    public AudioClip moveSound;
    private AudioSource audioSource;

    private GameObject[] Obstacles;
    private GameObject[] ObjToPush;
    private Vector3 lastPlayerPosition;
    private int playerMoveCount = 0;
    private int movesBeforeEnemyMove;
    private int stepsTaken = 0;
    private EnemyState currentState;

    // Define grid size (e.g., 0.5 units per grid cell)
    public float gridSize = 0.5f;

    private enum EnemyState
    {
        FollowPlayer,
        RandomMovement
    }

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned in EnemyMovementRandomized script.");
            return;
        }

        Obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        ObjToPush = GameObject.FindGameObjectsWithTag("ObjToPush");
        lastPlayerPosition = player.position;
        audioSource = GetComponent<AudioSource>();

        RandomizeMovesBeforeEnemyMove();
        SwitchState();
    }

    void Update()
    {
        if (Vector3.Distance(player.position, lastPlayerPosition) >= 1.0f)
        {
            playerMoveCount++;
            lastPlayerPosition = player.position;

            if (playerMoveCount >= movesBeforeEnemyMove)
            {
                MoveEnemy();
                playerMoveCount = 0;
                RandomizeMovesBeforeEnemyMove();
            }
        }
    }

    void MoveEnemy()
    {
        switch (currentState)
        {
            case EnemyState.FollowPlayer:
                FollowPlayer();
                break;

            case EnemyState.RandomMovement:
                RandomMove();
                break;
        }

        stepsTaken++;

        if (stepsTaken >= 10)
        {
            stepsTaken = 0;
            SwitchState();
        }
    }

    void FollowPlayer()
    {
        Vector3 direction = player.position - transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            direction = new Vector3(Mathf.Sign(direction.x), 0, 0);
        }
        else
        {
            direction = new Vector3(0, Mathf.Sign(direction.y), 0);
        }

        Vector3 newPosition = transform.position + direction;

        // Check if the new position would overlap the player's position
        if (newPosition == player.position)
        {
            // Enemy stays in its current position if the path is blocked by the player
            return;
        }

        // Attempt to move to the new position if it's valid
        if (CanMoveInDirection(direction))
        {
            transform.position = SnapToGrid(newPosition);
            PlayMoveSound();
        }
        else
        {
            TryRandomDirection();
        }
    }

    void RandomMove()
    {
        Vector3 direction = Vector3.zero;
        int randomDirection = Random.Range(0, 4);

        switch (randomDirection)
        {
            case 0: direction = Vector3.up; break;
            case 1: direction = Vector3.down; break;
            case 2: direction = Vector3.left; break;
            case 3: direction = Vector3.right; break;
        }

        Vector3 newPosition = transform.position + direction;

        // Check if the new position would overlap the player's position
        if (newPosition == player.position)
        {
            // Enemy stays in its current position if the path is blocked by the player
            return;
        }

        if (CanMoveInDirection(direction))
        {
            transform.position = SnapToGrid(newPosition);
            PlayMoveSound();
        }
        else
        {
            TryRandomDirection();
        }
    }

    void TryRandomDirection()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 direction = Vector3.zero;
            int randomDirection = Random.Range(0, 4);

            switch (randomDirection)
            {
                case 0: direction = Vector3.up; break;
                case 1: direction = Vector3.down; break;
                case 2: direction = Vector3.left; break;
                case 3: direction = Vector3.right; break;
            }

            Vector3 newPosition = transform.position + direction;

            // Check if the new position would overlap the player's position
            if (newPosition == player.position)
            {
                // Enemy stays in its current position if the path is blocked by the player
                return;
            }

            if (CanMoveInDirection(direction))
            {
                transform.position = SnapToGrid(newPosition);
                PlayMoveSound();
                return;
            }
        }
    }

    bool CanMoveInDirection(Vector3 direction)
    {
        Vector3 newPosition = transform.position + direction;

        if (Blocked(newPosition, direction))
        {
            return false;
        }

        return true;
    }

    bool Blocked(Vector3 position, Vector3 direction)
    {
        Vector3 newpos = position + direction;

        foreach (var obj in Obstacles)
        {
            if (SnapToGrid(obj.transform.position) == SnapToGrid(newpos))
            {
                return true;
            }
        }

        foreach (var objToPush in ObjToPush)
        {
            if (SnapToGrid(objToPush.transform.position) == SnapToGrid(newpos))
            {
                return true;
            }
        }

        return false;
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        return new Vector3(x, y, position.z);
    }

    void PlayMoveSound()
    {
        if (moveSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(moveSound);
        }
    }

    void RandomizeMovesBeforeEnemyMove()
    {
        movesBeforeEnemyMove = Random.Range(1, 4); // Randomize between 1 and 3 moves
    }

    void SwitchState()
    {
        currentState = (Random.value > 0.5f) ? EnemyState.FollowPlayer : EnemyState.RandomMovement;
    }
}