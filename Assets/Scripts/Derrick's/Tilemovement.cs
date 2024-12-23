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
    public static event System.Action OnPlayerMove; 

    

    private bool ReadyToMove;
    
    void Start()
    {
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacles");
        ObjToPush = GameObject.FindGameObjectsWithTag("ObjToPush");
        audioSource = GetComponent<AudioSource>();
        checkpointManager = CheckpointManager.Instance;
    }

    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            Vector2 moveinput = new Vector2(
                Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Joystick Axis 6"), 
                Input.GetAxisRaw("Vertical") + Input.GetAxisRaw("Joystick Axis 7")    
            );            moveinput.Normalize();

            if (moveinput.sqrMagnitude > 0.5)
            {
                if (ReadyToMove)
                {
                    ReadyToMove = false;

                    
                    if (Move(moveinput))
                    {
                        PlayMoveSound(); 
                    }
                }
            }
            else
            {
                ReadyToMove = true;
            }

            /*if (Input.GetKeyDown(KeyCode.R))
            {
                SavePositionAndResetScene();
            }*/
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
            PlayMoveSound(); 
            OnPlayerMove?.Invoke(); 
            
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
        GameManager.Instance.SavePlayerPosition(transform.position);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }*/
}
