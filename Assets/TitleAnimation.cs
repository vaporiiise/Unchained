using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleAnimation : MonoBehaviour
{
    public TMP_Text textComponent;

    [Header("Shiver Settings")]
    public float baseShiverAmount = 0.5f; // Minimum shiver amount
    public float maxShiverAmount = 2f;    // Maximum shiver amount when close
    public float baseShiverSpeed = 10f;   // Minimum shiver speed
    public float maxShiverSpeed = 50f;    // Maximum shiver speed when close
    public float maxShiverDistance = 150f; // Maximum distance for shiver to increase

    private RectTransform textRectTransform;
    private Canvas canvas;
    private Camera mainCamera;

    private void Start()
    {
        textRectTransform = textComponent.GetComponent<RectTransform>();
        canvas = textComponent.canvas;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Get mouse position in world space
        Vector3 mouseWorldPosition = Input.mousePosition;
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, mainCamera.nearClipPlane));
        mouseWorldPosition.z = 0; // Ensure we're working in 2D

        Debug.Log($"Mouse world position: {mouseWorldPosition}");

        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            // Get the world position of the character's center
            Vector3 charBottomLeft = textComponent.transform.TransformPoint(verts[charInfo.vertexIndex]);
            Vector3 charTopRight = textComponent.transform.TransformPoint(verts[charInfo.vertexIndex + 2]);
            Vector3 charCenter = (charBottomLeft + charTopRight) / 2f;

            // Log the character's center position
            Debug.Log($"Character {charInfo.character} position: {charCenter}");

            // Calculate the distance between the mouse and the character
            float distanceToMouse = Vector3.Distance(mouseWorldPosition, charCenter);
            Debug.Log($"Distance to mouse (Character {charInfo.character}): {distanceToMouse}");

            // Clamp the shiver intensity based on distance
            float shiverFactor = Mathf.Clamp01(1 - (distanceToMouse / maxShiverDistance));
            Debug.Log($"Shiver factor for Character {charInfo.character}: {shiverFactor}");

            // Calculate shiver amount and speed based on distance
            float currentShiverAmount = Mathf.Lerp(baseShiverAmount, maxShiverAmount, shiverFactor);
            float currentShiverSpeed = Mathf.Lerp(baseShiverSpeed, maxShiverSpeed, shiverFactor);

            Debug.Log($"Shiver amount for Character {charInfo.character}: {currentShiverAmount}");
            Debug.Log($"Shiver speed for Character {charInfo.character}: {currentShiverSpeed}");

            // Apply the shiver effect
            float timeOffset = Time.time * currentShiverSpeed + Random.Range(0f, 1f);
            float shiverAmountY = Mathf.Sin(timeOffset) * currentShiverAmount;
            float shiverAmountX = Mathf.Sin(timeOffset * 2f) * currentShiverAmount * 0.5f;

            for (int j = 0; j < 4; ++j)
            {
                verts[charInfo.vertexIndex + j] += new Vector3(shiverAmountX, shiverAmountY, 0);
            }
        }

        // Update the mesh with the new vertex positions
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
