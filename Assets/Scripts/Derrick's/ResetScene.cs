using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    public int Scenes = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Reload the currently active scene
            SceneManager.LoadScene(Scenes);
        }
    }
}
