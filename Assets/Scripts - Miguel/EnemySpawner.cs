using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    [Tooltip("Timer Display")]
    public TextMeshProUGUI countdownText;

    [Tooltip("Enemy Kills Display")]
    public TextMeshProUGUI enemyDeathText;

    private int enemiesKilled = 0;
    private float remainingTime;
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

        remainingTime = totalSpawnTime;
        spawnCoroutine = StartCoroutine(SpawnEnemies());
        StartCoroutine(CountdownTimer());
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

    private IEnumerator CountdownTimer()
    {
        while (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            string timeString = $"{minutes:00}:{seconds:00}";

            if (countdownText != null)
            {
                countdownText.text = $"{timeString}";
            }

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        if (countdownText != null)
        {
            countdownText.text = "Time's Up!";
        }
    }

    private void SpawnEnemy()
    {
        Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newEnemy = Instantiate(enemyPrefab, randomSpawn.position, randomSpawn.rotation);
        activeEnemies.Add(newEnemy);
    }

    public void EnemyDied()
    {
        enemiesKilled++;

        if (enemyDeathText != null)
        {
            enemyDeathText.text = $"Enemies Killed:\n {enemiesKilled}";
        }
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
