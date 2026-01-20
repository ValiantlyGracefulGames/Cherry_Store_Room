using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleTimer : MonoBehaviour
{
    public Animator sparkleAnimator;

    public float minTime = 2.5f;
    public float maxTime = 3.5f;

    public float xOffsetRange = 0.15f; // how far left/right it can move

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
        InvokeRepeating(nameof(PlaySparkle), Random.Range(minTime, maxTime), Random.Range(minTime, maxTime));
    }

    void PlaySparkle()
    {
        float randomX = Random.Range(-xOffsetRange, xOffsetRange);
        transform.localPosition = startPosition + new Vector3(randomX, 0f, 0f);

        sparkleAnimator.Play("SparkleAnim", 0, 0f);
    }
}