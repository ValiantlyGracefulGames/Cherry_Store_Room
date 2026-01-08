using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float minRespawnDelay = 0.2f;
    public float maxRespawnDelay = 0.5f;

    public bool CanSpawn { get; private set; } = true;

    private int blockingObjects = 0;
    private Coroutine cooldownRoutine;

    public bool IsOccupied => blockingObjects > 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("IceCream") || other.CompareTag("Splat"))
        {
            blockingObjects++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("IceCream") || other.CompareTag("Splat"))
        {
            blockingObjects = Mathf.Max(0, blockingObjects - 1);

            // Only start cooldown if object active
            if (blockingObjects == 0 && cooldownRoutine == null && gameObject.activeInHierarchy)
            {
                cooldownRoutine = StartCoroutine(RespawnCooldown());
            }
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
        // Stop the coroutine if disabled
        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }

        blockingObjects = 0;
        CanSpawn = true;
    }
}