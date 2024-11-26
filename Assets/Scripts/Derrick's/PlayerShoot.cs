using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab for the bullet
    public Transform bulletsParent; // Parent for organizing bullets
    public SpriteRenderer directionIndicator; // SpriteRenderer to show the selected direction
    public Sprite upSprite, downSprite, leftSprite, rightSprite; // Direction sprites

    private Vector2Int selectedDirection; // Current selected direction
    private bool isDirectionSelected = false; // Flag to check if direction is selected
    public GameObject rouletteWheel;

    void Update()
    {
        // Start roulette on R key press
        if (Input.GetKeyDown(KeyCode.F) && !isDirectionSelected)
        {
            StartCoroutine(StartRouletteWithAnimation());
        }

        // Shoot when direction is selected and Space is pressed
        if (Input.GetKeyDown(KeyCode.Space) && isDirectionSelected)
        {
            rouletteWheel.SetActive(false);

            Shoot();
            ResetIndicator();
        }
    }
    
    IEnumerator StartRouletteWithAnimation()
    {
        // Activate the roulette wheel for the animation
        rouletteWheel.SetActive(true);

        // Wait for a few seconds to let the animation play
        yield return new WaitForSeconds(2f); // Adjust the duration as needed

        // Start the roulette selection
        StartRoulette();
    }



    void StartRoulette()
    {

        
        isDirectionSelected = true;

        // Randomly choose a direction
        int randomDir = Random.Range(0, 4);
        switch (randomDir)
        {
            case 0: // Up
                selectedDirection = Vector2Int.up;
                directionIndicator.sprite = upSprite;
                break;
            case 1: // Down
                selectedDirection = Vector2Int.down;
                directionIndicator.sprite = downSprite;
                break;
            case 2: // Left
                selectedDirection = Vector2Int.left;
                directionIndicator.sprite = leftSprite;
                break;
            case 3: // Right
                selectedDirection = Vector2Int.right;
                directionIndicator.sprite = rightSprite;
                break;
        }

        // Activate the indicator
        directionIndicator.gameObject.SetActive(true);
    }

    void Shoot()
    {
        if (bulletPrefab == null || bulletsParent == null) return;

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, bulletsParent);

        // Set the direction directly
        TVBullet bulletScript = bullet.GetComponent<TVBullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(selectedDirection); // Pass the selected direction
        }

        // Reset the state
        isDirectionSelected = false;
    }
    

    void ResetIndicator()
    {
        // Deactivate the direction indicator after shooting
        directionIndicator.gameObject.SetActive(false);
    }
}
