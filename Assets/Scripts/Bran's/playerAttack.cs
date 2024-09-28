using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public int maxCombo = 5;
    public float comboResetTime = 1.5F;

    private int currentCombo = 0;
    private float lastAttackTime = 0F;

    public float hitBoxActiveDuration = 0.2F;
    public GameObject attackHitBox;

    private void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastAttackTime > comboResetTime)
                currentCombo = 0;

            StartCoroutine(ActiveHitBox(direction));
        }
    }

    IEnumerator ActiveHitBox(Vector2 attackDirection)
    {
        attackHitBox.SetActive(true);
        attackHitBox.transform.position = (Vector2)transform.position + attackDirection * 1.0F;
        yield return new WaitForSeconds(hitBoxActiveDuration);
        attackHitBox.SetActive(false);
    }
}