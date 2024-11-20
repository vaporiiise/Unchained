using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ArchivesOnClick : MonoBehaviour
{
   public float cameraMoveDuration = 1.5f;
   public Vector3 cameraTargetPos;
   public float smoothTime = 0.2f;
   public GameObject moveButtons;
   public float objectMoveDuration = 0.2f;
   public float targetXPos = 10f;
   public GameObject itemToShow;
   public float zPosItem;
   private Vector3 originalPos;
   public float smoothZ = 0.2f;
   

   private void Start()
   {
      originalPos = transform.localPosition;
   }
   
   public void OnButtonClick()
   {
      StartCoroutine(MoveCameraToTarget());
      StartCoroutine(MoveButtons());

   }

   private IEnumerator MoveCameraToTarget()
   {
      Camera mainCamera = Camera.main;
      Vector3 velocity = Vector3.zero; // Required by SmoothDamp to smoothly reach the target position
      float elapsedTime = 0f;

      // Smoothly move the camera to the target position using SmoothDamp
      while (elapsedTime < cameraMoveDuration)
      {
         mainCamera.transform.position =
            Vector3.SmoothDamp(mainCamera.transform.position, cameraTargetPos, ref velocity, smoothTime);
         elapsedTime += Time.deltaTime;
         yield return null;
      }

      // Ensure the camera reaches the exact target position
      mainCamera.transform.position = cameraTargetPos;
      
      
   }

   private IEnumerator MoveButtons()
   {
      float elapsedTime = 0f;
      Vector3 startingPosition = moveButtons.transform.position;
      Vector3 targetPosition = new Vector3(targetXPos, startingPosition.y, startingPosition.z);

      while (elapsedTime < objectMoveDuration)
      {
         elapsedTime += Time.deltaTime;
         float t = elapsedTime / objectMoveDuration;
         t = t * t * (3f - 2f * t); // Smoothstep interpolation for natural movement
         moveButtons.transform.position = Vector3.Lerp(startingPosition, targetPosition, t);
         yield return null;
      }

      moveButtons.transform.position = targetPosition;
   }

   private IEnumerator ShowItem(Vector3 targetPos)
   {
      float elapsedTime = 0f;
      Vector3 startingPosition = itemToShow.transform.localPosition;

      while (elapsedTime < smoothZ)
      {
         itemToShow.transform.localScale = Vector3.Lerp(startingPosition, targetPos, elapsedTime / smoothZ);
         elapsedTime += Time.deltaTime;
         yield return null;
      }
      itemToShow.transform.localScale = targetPos;

   }
}
