using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform spawnPoint;

    void Start()
    {
        int index = PlayerSelectionData.SelectedPlayerIndex;

        GameObject spawnedPlayer = Instantiate(playerPrefabs[index], spawnPoint.position, Quaternion.identity
        );

        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.player = spawnedPlayer.transform;
        }
    }
}
