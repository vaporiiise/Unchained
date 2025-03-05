using System.Collections;
using UnityEngine;

public class CanvasSwaper : MonoBehaviour
{
    public Canvas currentCanvas; // The canvas to hide
    public Canvas targetCanvas;  // The canvas to show

    void Start()
    {
        if (targetCanvas == null)
        {
            Debug.LogWarning("MenuSettingsCanvas: No target canvas assigned on " + gameObject.name);
        }
    }

    public void SwapCanvas()
    {
        if (currentCanvas != null) currentCanvas.gameObject.SetActive(false);
        if (targetCanvas != null) targetCanvas.gameObject.SetActive(true);
    }
}
