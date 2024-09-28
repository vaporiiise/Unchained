using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload the currently active scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
