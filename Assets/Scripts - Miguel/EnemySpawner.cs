using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Tooltip("Total time during which enemies will spawn.")]
    public float totalSpawnTime = 30f;

    [Tooltip("Initial time between enemy spawns.")]
    public float initialSpawnInterval = 2f;

    [Tooltip("Minimum time between spawns as spawning accelerates.")]
    public float minimumSpawnInterval = 0.5f;

    private float elapsedTime = 0f;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Coroutine spawnCoroutine;

    private void Start()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Enemy prefab or spawn points not assigned.");
            return;
        }

        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (elapsedTime < totalSpawnTime)
        {
            float t = elapsedTime / totalSpawnTime;
            float currentInterval = Mathf.Lerp(initialSpawnInterval, minimumSpawnInterval, t);

            SpawnEnemy();

            yield return new WaitForSeconds(currentInterval);
            elapsedTime += currentInterval;
        }
    }

    private void SpawnEnemy()
    {
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawn.position, randomSpawn.rotation);
        activeEnemies.Add(newEnemy);
    }

    /// <summary>
    /// Destroys all currently spawned enemies.
    /// </summary>
    public void DespawnAllEnemies()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        activeEnemies.Clear();

        // Optional: Stop spawning if you want to fully end the process
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        elapsedTime = 0f;
    }
}
