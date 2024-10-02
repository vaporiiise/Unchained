using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTest : MonoBehaviour
{
    public int health = 40;

    private void OnTriggerEnter2D(Collider2D enemyCol)
    {
        if (enemyCol.gameObject.CompareTag("PlayerAttack"))
            TakeDamage(10);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Health remaining: {health}");

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}