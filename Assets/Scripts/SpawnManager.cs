using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnPoint spawnPoint1;
    public SpawnPoint spawnPoint2;

    public GameObject[] iceCreamTubPrefabs;
    public float spawnInterval = 10f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            TrySpawnTub();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void TrySpawnTub()
    {
        SpawnPoint chosenPoint = null;

        if (!spawnPoint1.IsOccupied && spawnPoint1.CanSpawn)
            chosenPoint = spawnPoint1;
        else if (!spawnPoint2.IsOccupied && spawnPoint2.CanSpawn)
            chosenPoint = spawnPoint2;
        else
            return;

        GameObject prefab =
            iceCreamTubPrefabs[Random.Range(0, iceCreamTubPrefabs.Length)];

        Instantiate(prefab, chosenPoint.transform.position, Quaternion.identity);
    }
}