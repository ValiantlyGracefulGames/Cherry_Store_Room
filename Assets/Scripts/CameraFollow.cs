using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 newPos = player.position;
        newPos.z = -10f; // keep camera behind player
        transform.position = newPos;
    }
}
