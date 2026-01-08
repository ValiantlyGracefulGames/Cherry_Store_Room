using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class IceCreamTub : MonoBehaviour
{
    
    public Flavor flavor;

    public Sprite phase1Sprite;
    public Sprite phase2Sprite;
    public Sprite phase3Sprite;

    public float totalLifeTime = 20f;
    private float timer;

    public GameObject splatPrefab;
    private SpriteRenderer sr;

    private bool hasMelted = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timer = totalLifeTime;
        sr.sprite = phase1Sprite;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        float difficulty = GameManager.Instance.GetDifficultyMultiplier();
        timer -= Time.deltaTime * difficulty;

        float phaseTime = totalLifeTime / 3f;

        if (timer > 2 * phaseTime)
            sr.sprite = phase1Sprite;
        else if (timer > phaseTime)
            sr.sprite = phase2Sprite;
        else if (timer > 0)
            sr.sprite = phase3Sprite;
        else if (!hasMelted)
        {
            Melt();
        }
    }

    void Melt()

    {
        if (hasMelted) return;
        hasMelted = true;

        // Spawn splat if prefab exists
        if (splatPrefab != null)
        {
            Instantiate(splatPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    // Called when player picks it up
    public void OnPickedUp()
    {
        // ❗ DO NOTHING HERE regarding spawn point
        // Tub still counts as occupying until it melts
    }
}