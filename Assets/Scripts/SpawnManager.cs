using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnPoint spawnPoint1;
    public SpawnPoint spawnPoint2;

    public GameObject[] iceCreamTubPrefabs;

    public float minSpawnInterval = 2f;
    public float maxSpawnInterval = 6;

    [Header("Difficulty Scaling")]
    public float maxDifficultyMultiplier = 2f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            TrySpawnTub();
            float difficulty = GameManager.Instance.GetDifficultyMultiplier();
            float delay = Random.Range(minSpawnInterval, maxSpawnInterval) / difficulty;

            yield return new WaitForSeconds(delay);
        }
    }

    void TrySpawnTub()
    {
        List<SpawnPoint> availablePoints = new List<SpawnPoint>();

        if (!spawnPoint1.IsOccupied && spawnPoint1.CanSpawn)
            availablePoints.Add(spawnPoint1);

        if (!spawnPoint2.IsOccupied && spawnPoint2.CanSpawn)
            availablePoints.Add(spawnPoint2);

        if (availablePoints.Count == 0)
            return;

        SpawnPoint chosenPoint =
            availablePoints[Random.Range(0, availablePoints.Count)];

        GameObject prefab =
            iceCreamTubPrefabs[Random.Range(0, iceCreamTubPrefabs.Length)];

        Instantiate(
            prefab,
            chosenPoint.transform.position,
            Quaternion.identity
        );
    }
}