using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSprinkleSpawner : MonoBehaviour
{
    public GameObject sprinklePrefab;
    public Sprite[] sprinkleSprites;

    public float spawnInterval = 1f;
    public float minSpeed = 0.2f;
    public float maxSpeed = 0.5f;

    public float spawnY = 7.5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnSprinkle), 0f, spawnInterval);
    }

    void SpawnSprinkle()
    {
        float x;

        // 50/50 chance: left or right side
        if (Random.value < 0.5f)
        {
            // Left side
            x = Random.Range(-15f, -7.5f);
        }
        else
        {
            // Right side
            x = Random.Range(7f, 15f);
        }

        Vector3 spawnPos = new Vector3(x, spawnY, 0f);

        GameObject obj = Instantiate(sprinklePrefab, spawnPos, Quaternion.identity, transform);

        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sprite = sprinkleSprites[Random.Range(0, sprinkleSprites.Length)];

        // Optional: soften the look
        sr.color = new Color(1f, 1f, 1f, 0.6f);

        BackgroundSprinkle bs = obj.GetComponent<BackgroundSprinkle>();
        bs.fallSpeed = Random.Range(minSpeed, maxSpeed);

        // Optional scale variation
        float scale = Random.Range(0.8f, 1.2f);
        obj.transform.localScale = Vector3.one * scale;
    }
}

