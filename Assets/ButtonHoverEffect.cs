using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f);  // Scale to enlarge on hover
    public float animationDuration = 0.2f;                      // Duration for scale animation

    private Vector3 originalScale;
    private bool isHovering = false;

    private void Start()
    {
        originalScale = transform.localScale;  // Store the original scale of the button
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Enlarge button when the pointer enters
        StopAllCoroutines();
        StartCoroutine(ScaleButton(hoverScale));
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Return button to original size when the pointer exits
        StopAllCoroutines();
        StartCoroutine(ScaleButton(originalScale));
        isHovering = false;
    }

    private IEnumerator ScaleButton(Vector3 targetScale)
    {
        float elapsedTime = 0f;
        Vector3 startingScale = transform.localScale;

        while (elapsedTime < animationDuration)
        {
            transform.localScale = Vector3.Lerp(startingScale, targetScale, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
