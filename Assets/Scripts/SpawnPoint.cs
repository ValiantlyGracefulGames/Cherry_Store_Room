using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Respawn Delay")]
    public float minRespawnDelay = 7f;
    public float maxRespawnDelay = 12f;

    public bool IsOccupied { get; private set; }
    public bool CanSpawn { get; private set; } = true;

    private int objectCount = 0;
    private Coroutine cooldownRoutine;

    // Called by SpawnManager immediately after spawning
    public void RegisterSpawnedObject()
    {
        objectCount++;
        IsOccupied = true;

        // Stop cooldown if something spawns early
        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }
    }

    // Called by IceCreamTub AFTER tub + splat are fully gone
    public void NotifyObjectRemoved()
    {
        objectCount = Mathf.Max(0, objectCount - 1);
        IsOccupied = objectCount > 0;

        if (!IsOccupied && cooldownRoutine == null && gameObject.activeInHierarchy)
        {
            cooldownRoutine = StartCoroutine(RespawnCooldown());
        }
    }

    private IEnumerator RespawnCooldown()
    {
        CanSpawn = false;

        float delay = Random.Range(minRespawnDelay, maxRespawnDelay);
        yield return new WaitForSeconds(delay);

        CanSpawn = true;
        cooldownRoutine = null;
    }

    private void OnDisable()
    {
        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }

        objectCount = 0;
        IsOccupied = false;
        CanSpawn = true;
    }
}