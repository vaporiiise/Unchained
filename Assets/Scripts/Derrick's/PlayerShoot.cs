using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletSprite;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float bulletDowntime = 0.2F;

    private bool isShooting = false;

   
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

        if (Input.GetMouseButtonDown(0))
            StartShooting();
        
        else if (Input.GetMouseButtonUp(0))
            StopShooting();
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletSprite, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = firePoint.up * bulletSpeed;
        Debug.Log("Shooting..");
    }

    void StartShooting()
    {
        if (!isShooting)
        {
            isShooting = true;
            InvokeRepeating("Shoot", 0F, bulletDowntime);
        }
    }

    void StopShooting()
    {
        isShooting = false;
        CancelInvoke("Shoot");
    }

   
}
