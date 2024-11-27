using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerParry : MonoBehaviour
{
    public float parryDuration = 0.5F;
    public float parryCooldown = 3F;

    private bool isParrying = false;
    private bool canParry = true;

    public GameObject parryHitbox;

    private playerMovement movementScript;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && canParry)
            StartCoroutine(ParryWindow());
    }

    IEnumerator ParryWindow()
    {
        canParry = false;
        isParrying = true;

        Debug.Log("Parry Window Up!");
        parryHitbox.SetActive(true);
        parryHitbox.transform.position = (Vector2)transform.position * 1.0F;
        movementScript.EnableMovement(false);
        yield return new WaitForSeconds(parryDuration);
        parryHitbox.SetActive(false);
        Debug.Log("Parry Window Down!");
        isParrying = false;

        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
        movementScript.EnableMovement(true);
    }

    private void OnTriggerEnter2D(Collider2D parryCol)
    {
       if (isParrying && parryCol.CompareTag("BossAttack"))
        {
            Debug.Log("Parried!");

            bossAI boss = parryCol.GetComponentInParent<bossAI>();
            if (boss != null)
                boss.TakeDamage(30);
        }
    }
}