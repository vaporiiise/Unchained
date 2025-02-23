using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RemoveSavedPosition : MonoBehaviour
{
    [MenuItem("Tools/Clear Saved Player Position")]
    public static void ClearPosition()
    {
        if (EditorUtility.DisplayDialog("Clear Saved Position",
                "Are you sure you want to remove the saved player position?", "Yes", "No"))
        {
            PlayerPrefs.DeleteKey("SavedX");
            PlayerPrefs.DeleteKey("SavedY");
            PlayerPrefs.Save();
            Debug.Log("Saved player position cleared!");
        }
    }
}
