using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBoxSpawner : MonoBehaviour
{
    public GameObject positionAreaPrefab;
    public GameObject particleEffectPrefab;
    public string targetTag = "ObjToPush"; // The tag of objects that trigger a new position spawn

    public Vector2 gridSize = new Vector2(5, 5);
    public Vector3 gridOffset = Vector3.zero;
    public float gridCellSize = 1f;
    public float effectDuration = 1.5f;

    private GameObject positionArea;
    private Vector3 targetPosition;

    void Start()
    {
        SpawnPositionArea();
    }

    void Update()
    {
        if (AnyTaggedObjectInTargetPosition())
        {
            StartCoroutine(HandlePositionReached());
        }
    }

    IEnumerator HandlePositionReached()
    {
        // Play particle effect at position area before destroying it
        GameObject effect = Instantiate(particleEffectPrefab, targetPosition, Quaternion.identity);
        yield return new WaitForSeconds(effectDuration); // Wait for effect to finish

        // Destroy the position area and particle effect
        Destroy(positionArea);
        Destroy(effect);

        // Immediately spawn a new position area
        SpawnPositionArea();
    }

    void SpawnPositionArea()
    {
        targetPosition = GetRandomPositionInGridCell();
        positionArea = Instantiate(positionAreaPrefab, targetPosition, Quaternion.identity);
        Instantiate(particleEffectPrefab, targetPosition, Quaternion.identity);
    }

    bool AnyTaggedObjectInTargetPosition()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject obj in objects)
        {
            if (Vector3.Distance(obj.transform.position, targetPosition) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    Vector3 GetRandomPositionInGridCell()
    {
        int x = Random.Range(0, (int)gridSize.x);
        int y = Random.Range(0, (int)gridSize.y);

        // Center the position area in the middle of the selected grid cell
        float centerX = (x + 0.5f) * gridCellSize;
        float centerY = (y + 0.5f) * gridCellSize;

        return new Vector3(centerX, centerY, 0) + gridOffset;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        for (int x = 0; x <= gridSize.x; x++)
        {
            Gizmos.DrawLine(gridOffset + new Vector3(x * gridCellSize, 0, 0), gridOffset + new Vector3(x * gridCellSize, gridSize.y * gridCellSize, 0));
        }
        for (int y = 0; y <= gridSize.y; y++)
        {
            Gizmos.DrawLine(gridOffset + new Vector3(0, y * gridCellSize, 0), gridOffset + new Vector3(gridSize.x * gridCellSize, y * gridCellSize, 0));
        }

        if (positionArea != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(positionArea.transform.position, 0.2f * gridCellSize);
        }
    }
}