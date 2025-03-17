using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationShowcaseManager : MonoBehaviour
{
    public List<GameObject> items; // Assign animated GameObjects in Inspector
    private int currentIndex = 0;

    void Start()
    {
        UpdateShowcase();
    }

    public void NextItem()
    {
        currentIndex = (currentIndex + 1) % items.Count;
        UpdateShowcase();
    }

    public void PreviousItem()
    {
        currentIndex = (currentIndex - 1 + items.Count) % items.Count;
        UpdateShowcase();
    }

    void UpdateShowcase()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Animator animator = items[i].GetComponent<Animator>();
            SpriteRenderer sprite = items[i].GetComponent<SpriteRenderer>();

            if (i == currentIndex)
            {
                // Activate and play the selected animation
                items[i].SetActive(true);
                animator.enabled = true;
                sprite.color = Color.white; // Full brightness
            }
            else
            {
                // Darken and stop other animations
                animator.enabled = false;
                sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f); // Dark color
            }
        }
    }
}
