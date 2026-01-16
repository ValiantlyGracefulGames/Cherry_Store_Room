using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSprinkle : MonoBehaviour
{
    public float fallSpeed = 0.2f;
    public float driftAmount = 0.05f;

    float drift;

    void Start()
    {
        drift = Random.Range(-driftAmount, driftAmount);
    }

    void Update()
    {
        transform.position += new Vector3(drift, -fallSpeed, 0f) * Time.deltaTime;

        // Destroy once far below the screen
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }
}