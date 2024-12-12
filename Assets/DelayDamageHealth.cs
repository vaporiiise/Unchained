using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayDamageHealth : MonoBehaviour
{
    public Slider mainHealthBarSlider;
    public Slider damagedHealthBarSlider;

    public bossAI enemyScript;


    public float delayTime = 1f;        // Delay time before health bar starts following (in seconds)
    public float smoothSpeed = 2f;      // Speed at which the damaged health bar follows the main health bar

    private float targetHealth;            // Target health value to be set for the damaged health bar
    private float previousHealth;          // To track the previous health value
    private float damageTimer;             // Timer to track the time since last damage
    private bool isDamageReceived;         // Whether the enemy is receiving damage

    private void Start()
    {
        if (mainHealthBarSlider != null && enemyScript != null)
        {
            mainHealthBarSlider.maxValue = enemyScript.bossMaxHealth;
            damagedHealthBarSlider.maxValue = enemyScript.bossMaxHealth;
        }
        else
        {
            Debug.LogError("Missing reference to mainHealthBarSlider or enemyScript.");
        }

        targetHealth = mainHealthBarSlider.value;
        previousHealth = mainHealthBarSlider.value;
        damageTimer = delayTime;
        isDamageReceived = false;
    }

    private void Update()
    {
        if (mainHealthBarSlider == null || damagedHealthBarSlider == null || enemyScript == null)
        {
            Debug.LogError("Missing reference to sliders or enemyScript.");
            return;
        }

        if (mainHealthBarSlider.value < previousHealth)
        {
            isDamageReceived = true;
            damageTimer = delayTime;
        }
        else
        {
            isDamageReceived = false;
        }

        if (!isDamageReceived)
        {
            damageTimer -= Time.deltaTime;
            damageTimer = Mathf.Max(damageTimer, 0f);

            if (damageTimer <= 0f)
            {
                StartCoroutine(SmoothHealthUpdate());
            }
        }

        previousHealth = mainHealthBarSlider.value;

        Debug.Log($"Main Health: {mainHealthBarSlider.value}, Damaged Health: {damagedHealthBarSlider.value}, Damage Timer: {damageTimer}");
    }

    private IEnumerator SmoothHealthUpdate()
    {
        float startHealth = damagedHealthBarSlider.value;
        float targetHealth = mainHealthBarSlider.value;
        float elapsedTime = 0f;

        while (Mathf.Abs(damagedHealthBarSlider.value - targetHealth) > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = elapsedTime * smoothSpeed;

            damagedHealthBarSlider.value = Mathf.Lerp(startHealth, targetHealth, lerpValue);

            yield return null;
        }

        damagedHealthBarSlider.value = targetHealth;
    }
}