using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryEffect : MonoBehaviour
{
    public float lifetime = 2.5f; // seconds before it disappears

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}