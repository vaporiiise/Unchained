using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton instance

    public Vector3 savedPlayerPosition; // Store the player's position here

    private void Awake()
    {
        // Implement the Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    // Method to save the player's position
    public void SavePlayerPosition(Vector3 position)
    {
        savedPlayerPosition = position;
        Debug.Log("Player position saved: " + savedPlayerPosition);
    }
}
