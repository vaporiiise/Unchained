using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    [Header("Jump Attack Settings")]
    [SerializeField] private float jumpAttackDuration = 0.5f;
    [SerializeField] private float jumpAttackMagnitude = 0.3f;

    [Header("Ground Slam Settings")]
    [SerializeField] private float groundSlamDuration = 0.6f;
    [SerializeField] private float groundSlamMagnitude = 0.35f;

    [Header("Hand Slam Settings")]
    [SerializeField] private float handSlamDuration = 0.4f;
    [SerializeField] private float handSlamMagnitude = 0.2f;

    [Header("UI Shake Settings")]
    [SerializeField] private RectTransform uiElementToShake;
    [SerializeField] private float uiShakeDuration = 0.5f;
    [SerializeField] private float uiShakeMagnitude = 10f;

    private Vector3 shakeOffset;
    private FollowCamera followCameraScript; // Reference to FollowCamera script
    private bossAI bossScript; // Reference to boss script
    private playerAttack playerScript; // Reference to player script

    private bool isUIShaking = false;
    private int lastHealth = 0; // Track player's health from the last frame

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        bossScript = FindObjectOfType<bossAI>();
        followCameraScript = Camera.main.GetComponent<FollowCamera>();
        playerScript = FindObjectOfType<playerAttack>();

        if (playerScript != null)
            lastHealth = playerScript.currentHealth; // Initialize lastHealth
    }

    private void OnEnable()
    {
        shakeOffset = Vector3.zero;
        isUIShaking = false;
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            shakeOffset = new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        shakeOffset = Vector3.zero;
    }

    private void Update()
    {
        // Handle the boss's attack shake patterns
        if (bossScript != null)
        {
            if (bossScript.attackPattern == 3 && bossScript.handSwipeHitBox.activeInHierarchy)
            {
                Shake(jumpAttackDuration, jumpAttackMagnitude);
            }
            else if (bossScript.attackPattern == 2 && bossScript.groundSlamHitBox.activeInHierarchy)
            {
                Shake(groundSlamDuration, groundSlamMagnitude);
            }
            else if (bossScript.attackPattern == 1 && bossScript.handSlamHitBox.activeInHierarchy)
            {
                Shake(handSlamDuration, handSlamMagnitude);
            }

            if (playerScript != null && playerScript.currentHealth < lastHealth)
            {
                Debug.Log($"Player health decreased: {playerScript.currentHealth}");
            }

            if (isUIShaking)
            {
                Debug.LogWarning("UI Shake is already in progress.");
            }
        }

        // Trigger UI shake when the player's health decreases
        if (playerScript != null && playerScript.currentHealth < lastHealth && !isUIShaking)
        {
            isUIShaking = true;
            StartCoroutine(UIShakeCoroutine());
        }

        // Update lastHealth for the next frame
        if (playerScript != null)
            lastHealth = playerScript.currentHealth;

        // Apply screen shake offset to the camera
        if (shakeOffset != Vector3.zero && followCameraScript != null)
        {
            followCameraScript.transform.position += shakeOffset;
        }
    }

    private IEnumerator UIShakeCoroutine()
    {
        if (isUIShaking) yield break; // Prevent overlapping shakes
        isUIShaking = true;

        if (uiElementToShake == null)
        {
            Debug.LogError("UI Element to shake is null!");
            isUIShaking = false;
            yield break;
        }

        Debug.Log("UI Shake started.");
        Vector3 originalPosition = uiElementToShake.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < uiShakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * uiShakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * uiShakeMagnitude;

            uiElementToShake.anchoredPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        uiElementToShake.anchoredPosition = originalPosition;
        Debug.Log("UI Shake completed.");
        isUIShaking = false;
    }
}
