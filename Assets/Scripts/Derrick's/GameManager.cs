using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; 

    public Vector3 savedPlayerPosition; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void SavePlayerPosition(Vector3 position)
    {
        savedPlayerPosition = position;
        Debug.Log("Player position saved: " + savedPlayerPosition);
    }

   
}
