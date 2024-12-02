using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    [SerializeField] private float defaultShakeDuration = 0.5f;
    [SerializeField] private float defaultShakeMagnitude = 0.2f;

    private Vector3 originalPosition;
    private Vector3 shakeOffset;

    private FollowCamera followCameraScript;  // ref to FollowCamera script
    private bossAI bossScript; // ref to bossAI script

    private bool isShaking = false; // Whether the camera is currently shaking

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()//find where the scripts at
    {
        bossScript = FindObjectOfType<bossAI>();

        followCameraScript = Camera.main.GetComponent<FollowCamera>();
    }

    private void OnEnable()
    {
        originalPosition = transform.position;
    }

    // Trigger the camera shake
    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        float shakeDuration = duration > 0 ? duration : defaultShakeDuration;
        float shakeMagnitude = magnitude > 0 ? magnitude : defaultShakeMagnitude;
        StartCoroutine(ShakeCoroutine(shakeDuration, shakeMagnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsedTime = 0f;
        shakeOffset = Vector3.zero; // Reset shake offset

        while (elapsedTime < duration)
        {
            // Calculate random offsets for shake
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset shake offset to avoid lingering shake
        shakeOffset = Vector3.zero;
    }

    private void Update()
    {
        // Only check for the jump attack hitbox
        if (bossScript != null && bossScript.attackPattern == 3) // Jump Attack
        {
            // Check if the jump attack hitbox is active
            if (bossScript.handSwipeHitBox.activeInHierarchy)
            {
                // Trigger screen shake when the jump attack hitbox is active
                Shake(0.5f, 0.3f);
            }
        }

        // If the shake offset is not zero, apply it while following the player
        if (shakeOffset != Vector3.zero)
        {
            // Apply the shake offset on top of the camera's following position
            if (followCameraScript != null)
            {
                // Apply the shake offset to the camera position
                followCameraScript.transform.position += shakeOffset;
            }
        }
    }
}
