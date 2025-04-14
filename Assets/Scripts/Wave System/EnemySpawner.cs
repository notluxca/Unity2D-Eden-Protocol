using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Waves")]
    public Wave[] waves;
    public EnemyData[] possibleEnemies;

    [Header("Spawn")]
    public Transform[] spawnPoints;

    private int currentWaveIndex = 0;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    public string currentWaveName { get { return waves[currentWaveIndex].waveName; } }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave wave = waves[currentWaveIndex];
            Debug.Log($"Iniciando Wave {currentWaveIndex + 1}: {wave.enemyCount} inimigos");

            for (int i = 0; i < wave.enemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(wave.spawnDelay);
            }

            // Esperar todos os inimigos morrerem antes da próxima wave
            yield return new WaitUntil(() => aliveEnemies.Count == 0);

            currentWaveIndex++;
        }

        Debug.Log("Todas as waves finalizadas!");
    }

    void SpawnEnemy()
    {
        EnemyData enemyData = GetRandomEnemy();
        if (enemyData == null) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPoint.position, Quaternion.identity);
        aliveEnemies.Add(enemy);

        EnemyDeathHandler handler = enemy.AddComponent<EnemyDeathHandler>();
        handler.OnDeath += () => aliveEnemies.Remove(enemy);
    }

    EnemyData GetRandomEnemy()
    {
        float totalChance = 0f;
        foreach (var e in possibleEnemies) totalChance += e.spawnChance;

        float rand = Random.Range(0f, totalChance);
        float cumulative = 0f;

        foreach (var e in possibleEnemies)
        {
            cumulative += e.spawnChance;
            if (rand <= cumulative)
                return e;
        }

        return null;
    }
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public int enemyCount;
    public float spawnDelay;
}
