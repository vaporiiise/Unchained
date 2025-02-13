using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MenuController : MonoBehaviour
{
    public Button[] menuButtons; // Assign UI Buttons in Inspector
    private int selectedIndex = 0;

    private void Start()
    {
        HighlightButton();
    }

    private void Update()
    {
        // Move Down (↓ or S)
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            selectedIndex = (selectedIndex + 1) % menuButtons.Length;
            HighlightButton();
        }

        // Move Up (↑ or W)
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            selectedIndex = (selectedIndex - 1 + menuButtons.Length) % menuButtons.Length;
            HighlightButton();
        }

        // Select (Enter or Space)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            menuButtons[selectedIndex].onClick.Invoke(); // Trigger the selected button
        }
    }

    private void HighlightButton()
    {
        // Highlight the selected button
        EventSystem.current.SetSelectedGameObject(menuButtons[selectedIndex].gameObject);
    }
}
