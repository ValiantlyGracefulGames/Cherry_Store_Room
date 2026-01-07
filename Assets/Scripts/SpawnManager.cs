using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public SpawnPoint spawnPoint1;
    public SpawnPoint spawnPoint2;

    public GameObject[] iceCreamTubPrefabs;

    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 8;
    

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            TrySpawnTub();
            float delay = Random.Range(minSpawnInterval, maxSpawnInterval);
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

        GameObject tubGO =
            Instantiate(prefab, chosenPoint.transform.position, Quaternion.identity);

        // 🔗 Wire tub → spawn point
        IceCreamTub tub = tubGO.GetComponent<IceCreamTub>();
        if (tub != null)
        {
            tub.owningSpawnPoint = chosenPoint;
        }

        // 🔒 Mark spawn point occupied immediately
        chosenPoint.RegisterSpawnedObject();
    }
}