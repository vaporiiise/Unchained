using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public int currentCombo = 0;
    public float comboResetTime = 1F;
    private float comboDuration = 0F;

    public GameObject attack1HitBox;
    public GameObject attack2HitBox;
    public GameObject attack3HitBox;
    public GameObject attack4HitBox;

    public int health = 100;
    private Animator playerAnim;

    private void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the game is paused
        if (PauseMenu.GameIsPaused)
        {
            return; // Exit if paused
        }

        // Handle attack input
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;

        if (Input.GetMouseButtonDown(0)) // Left mouse button for attack
        {
            comboDuration = Time.time;

            currentCombo++;
            if (currentCombo > 4)
                currentCombo = 1;

            switch (currentCombo)
            {
                case 1:
                    StartCoroutine(Attack1HitBox(direction));
                    break;
                case 2:
                    StartCoroutine(Attack2HitBox(direction));
                    break;
                case 3:
                    StartCoroutine(Attack3HitBox(direction));
                    break;
                case 4:
                    StartCoroutine(Attack4HitBox(direction));
                    break;
            }
        }

        // Reset combo if the duration has exceeded the allowed time
        if (Time.time - comboDuration > comboResetTime)
            currentCombo = 0;
    }

    IEnumerator Attack1HitBox(Vector2 attackDirection)
    {
        playerAnim.SetTrigger("PlayerCombo1");
        yield return new WaitForSeconds(1F);
        attack1HitBox.SetActive(true);
        attack1HitBox.transform.position = (Vector2)transform.position + attackDirection * 1.0F;
        yield return new WaitForSeconds(0.2F);
        attack1HitBox.SetActive(false);
    }

    IEnumerator Attack2HitBox(Vector2 attackDirection)
    {
        attack2HitBox.SetActive(true);
        attack2HitBox.transform.position = (Vector2)transform.position + attackDirection * 1.0F;
        float startAngle = -45F;
        float endAngle = 45F;

        float elapsedTime = 0F;
        while (elapsedTime < 0.2F)
        {
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsedTime / 0.2F);
            attack2HitBox.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        attack2HitBox.SetActive(false);
    }

    IEnumerator Attack3HitBox(Vector2 attackDirection)
    {
        attack3HitBox.SetActive(true);
        attack3HitBox.transform.position = (Vector2)transform.position + attackDirection * 1.0F;
        float startAngle = 45F;
        float endAngle = -45F;

        float elapsedTime = 0F;
        while (elapsedTime < 0.2F)
        {
            float currentAngle = Mathf.Lerp(startAngle, endAngle, elapsedTime / 0.2F);
            attack3HitBox.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentAngle));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        attack3HitBox.SetActive(false);
    }

    IEnumerator Attack4HitBox(Vector2 attackDirection)
    {
        attack4HitBox.SetActive(true);
        attack4HitBox.transform.position = (Vector2)transform.position + attackDirection * 1.0F;
        yield return new WaitForSeconds(0.2F);
        attack4HitBox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D playerCol)
    {
        if (playerCol.gameObject.CompareTag("BossAttack"))
            TakeDamage(5);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Player Health: {health}");

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}