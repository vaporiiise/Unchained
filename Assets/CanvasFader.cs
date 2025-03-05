using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private int frameRate = 15; // 15 FPS effect
    private float fadeDuration = 0.5f; // 0.5 sec fade time

    void Awake()
    {
        // Ensure CanvasGroup exists
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvas(0, 1, null));
    }

    public void FadeOut(System.Action onComplete = null)
    {
        StartCoroutine(FadeCanvas(1, 0, onComplete));
    }

    public void SwapCanvasWithFade(Canvas currentCanvas, Canvas targetCanvas)
    {
        //StartCoroutine(FadeOut(() =>
        //{
        //    currentCanvas.gameObject.SetActive(false);
        //    targetCanvas.gameObject.SetActive(true);
        //    targetCanvas.GetComponent<CanvasFader>().FadeIn();
        //}));
    }

    private IEnumerator FadeCanvas(float startAlpha, float endAlpha, System.Action onComplete)
    {
        float frameTime = 1f / frameRate;
        int steps = Mathf.RoundToInt(fadeDuration * frameRate);
        float stepSize = 1f / steps;

        for (int i = 0; i < steps; i++)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, i * stepSize);
            yield return new WaitForSeconds(frameTime);
        }

        canvasGroup.alpha = endAlpha;
        onComplete?.Invoke(); // Callback to disable canvas if needed
    }
}
