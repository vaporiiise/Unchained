using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Tilemovement : MonoBehaviour
{
    private GameObject[] Obstacles;
    private GameObject[] ObjToPush;
    public AudioClip moveSound;
    public AudioSource audioSource;
    private CheckpointManager checkpointManager;
    public static event System.Action OnPlayerMove; // Event triggered when player moves

    

    private bool ReadyToMove;
    
    // Start is called before the first frame update
    void Start()
    {
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        ObjToPush = GameObject.FindGameObjectsWithTag("ObjToPush");
        audioSource = GetComponent<AudioSource>();
        checkpointManager = CheckpointManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // Only allow movement if the game is not paused
        if (!PauseMenu.GameIsPaused)
        {
            Vector2 moveinput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            moveinput.Normalize();

            if (moveinput.sqrMagnitude > 0.5)
            {
                if (ReadyToMove)
                {
                    ReadyToMove = false;

                    // Call Move() and check if movement was successful
                    if (Move(moveinput))
                    {
                        PlayMoveSound(); // Play move sound if movement was successful
                    }
                }
            }
            else
            {
                ReadyToMove = true;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                //SavePositionAndResetScene();
            }
        }
    }

    public bool Move(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) < 0.5)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }
        direction.Normalize();

        if (Blocked(transform.position, direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction);
            PlayMoveSound(); // Play move sound when the movement is successful
            OnPlayerMove?.Invoke(); // Trigger event
            
            return true;
        }
    }

    public bool Blocked(Vector3 position, Vector2 direction)
    {
        Vector2 newpos = new Vector2(position.x, position.y) + direction;

        foreach (var obj in Obstacles)
        {
            if (obj.transform.position.x == newpos.x && obj.transform.position.y == newpos.y)
            {
                return true;
            }

            foreach (var objToPush in ObjToPush)
            {
                if (objToPush.transform.position.x == newpos.x && objToPush.transform.position.y == newpos.y)
                {
                    Push objPush = objToPush.GetComponent<Push>();
                    if (objToPush && objPush.Move(direction))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void PlayMoveSound()
    {
        if (moveSound != null)
        {
            audioSource.PlayOneShot(moveSound);
        }
    }
    public void Respawn()
    {
        if (checkpointManager.GetCurrentCheckpoint() != Vector2.zero)
        {
            transform.position = checkpointManager.GetCurrentCheckpoint();
        }

        Debug.Log("Player respawned at: " + transform.position);
    }
    /*private void SavePositionAndResetScene()
    {
        // Save the player's current position to the GameManager
        GameManager.Instance.SavePlayerPosition(transform.position);

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }*/
}
