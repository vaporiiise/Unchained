using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SendPlayerBack : MonoBehaviour
{
    public GameObject player;       
    public float targetZPosition;   
    public Vector3 requiredPosition;  
    public float positionTolerance = 0.1f;
    public GameObject objectToDisable;
    public GameObject objectToDisable2;
    public Vector3 moveObject;
    public Light2D globalLight;
    public float intensity;
    public GameObject playerLight;

    void Update()
    {
        if (Vector3.Distance(player.transform.position, requiredPosition) <= positionTolerance)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Vector3 currentPosition = player.transform.position;
                player.transform.position = new Vector3(currentPosition.x, currentPosition.y, targetZPosition);
                objectToDisable.transform.position = Vector3.MoveTowards(objectToDisable.transform.position, moveObject, 10 * Time.deltaTime);
                globalLight.intensity = intensity;
                objectToDisable2.SetActive(false);
                playerLight.SetActive(true);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(requiredPosition, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(requiredPosition, positionTolerance);
    }
}
