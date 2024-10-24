using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private int sceneIndex; // The build index of the scene to load

    // Method to change the scene
    public void ChangeScene()
    {
        // Load the scene by its build index
        SceneManager.LoadScene(sceneIndex);
    }
}
