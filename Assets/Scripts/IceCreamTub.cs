using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class IceCreamTub : MonoBehaviour
{
    public enum Flavor { Chocolate, Strawberry, Vanilla }
    public Flavor flavor;

    public Sprite phase1Sprite;
    public Sprite phase2Sprite;
    public Sprite phase3Sprite;

    public float totalLifeTime = 20f;
    private float timer;

    public GameObject splatPrefab;

    private SpriteRenderer sr;

    public SpawnPoint owningSpawnPoint;

    public event Action OnTubRemoved; // fired when both tub & splat are gone

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timer = totalLifeTime;
        sr.sprite = phase1Sprite;

        OnTubRemoved += HandleTubRemoved;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        float phaseTime = totalLifeTime / 3f;

        if (timer > 2 * phaseTime)
            sr.sprite = phase1Sprite;
        else if (timer > phaseTime)
            sr.sprite = phase2Sprite;
        else if (timer > 0)
            sr.sprite = phase3Sprite;
        else
        {
            // Tub “dies” but we keep track of splat
            if (splatPrefab != null)
            {
                GameObject splat = Instantiate(splatPrefab, transform.position, Quaternion.identity);
              
                // Fire OnTubRemoved only AFTER splat is destroyed
                StartCoroutine(NotifyWhenSplatGone(splat));
            }
            else
            {
                OnTubRemoved?.Invoke();
            }

            Destroy(gameObject); // destroy the tub
        }
    }

    private System.Collections.IEnumerator NotifyWhenSplatGone(GameObject splat)
    {
        // Wait until the splat is gone
        while (splat != null)
            yield return null;

        OnTubRemoved?.Invoke();
    }

    void HandleTubRemoved()
    {
        if (owningSpawnPoint != null)
        {
            owningSpawnPoint.NotifyObjectRemoved();
        }
    }
}