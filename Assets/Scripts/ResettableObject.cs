using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResettableObject : MonoBehaviour, IResettable
{
    private Vector2 initialPosition;
    private Quaternion initialRotation;
    private bool initialActiveState;

    private void Start()
    {
        // Store initial state of the object
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialActiveState = gameObject.activeSelf;

        // Register the object with the GlobalResetManager
        GlobalResetManager.Instance.RegisterResettable(this);
    }

    // Reset the object to its initial state
    public void ResetState()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        gameObject.SetActive(initialActiveState);
        Debug.Log("Resetting: " + gameObject.name);
    }
}
