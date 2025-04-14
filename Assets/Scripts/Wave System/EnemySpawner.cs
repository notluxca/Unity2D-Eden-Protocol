using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Pool")]
    public EnemyData[] possibleEnemies;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private int currentWaveIndex = 0;
    private bool waveInProgress = false;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    public static Action onWaveStart, onWaveEnd;

    private Coroutine currentWaveCoroutine;

    void OnEnable()
    {
        DayCycleSystem.OnNightEnd += StopCurrentWave;
    }

    void OnDisable()
    {
        DayCycleSystem.OnNightEnd -= StopCurrentWave;
    }

    public void StartNextWave()
    {
        if (waveInProgress)
        {
            Debug.LogWarning("Wave já em andamento!");
            return;
        }

        currentWaveIndex++;
        Debug.Log($"Começando wave {currentWaveIndex}");
        waveInProgress = true;
        onWaveStart?.Invoke();
        currentWaveCoroutine = StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        int enemyCount = 5 + currentWaveIndex * 2; // Escala de dificuldade
        float spawnDelay = Mathf.Max(0.2f, 1.5f - currentWaveIndex * 0.1f); // Diminui o delay com waves

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemyBasedOnWave();
            yield return new WaitForSeconds(spawnDelay);
        }

        // Espera todos morrerem para considerar fim da wave
        yield return new WaitUntil(() => aliveEnemies.Count == 0);

        waveInProgress = false;
        onWaveEnd?.Invoke();
        Debug.Log($"Wave {currentWaveIndex} finalizada.");
    }

    void StopCurrentWave()
    {
        Debug.Log("Fim da noite, parando wave atual.");
        if (currentWaveCoroutine != null)
            StopCoroutine(currentWaveCoroutine);

        waveInProgress = false;
        onWaveEnd?.Invoke();
    }

    void SpawnEnemyBasedOnWave()
    {
        EnemyData enemyData = GetRandomEnemyForCurrentWave();
        if (enemyData == null) return;

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPoint.position, Quaternion.identity);
        aliveEnemies.Add(enemy);

        EnemyDeathHandler handler = enemy.AddComponent<EnemyDeathHandler>();
        handler.OnDeath += () => aliveEnemies.Remove(enemy);
    }

    EnemyData GetRandomEnemyForCurrentWave()
    {
        // Libera mais tipos de inimigos conforme avança nas waves
        int maxIndex = Mathf.Min(possibleEnemies.Length, 1 + currentWaveIndex / 3);
        var enemyPool = new List<EnemyData>();

        for (int i = 0; i < maxIndex; i++)
        {
            enemyPool.Add(possibleEnemies[i]);
        }

        float totalChance = 0f;
        foreach (var e in enemyPool) totalChance += e.spawnChance;

        float rand = UnityEngine.Random.Range(0f, totalChance);
        float cumulative = 0f;

        foreach (var e in enemyPool)
        {
            cumulative += e.spawnChance;
            if (rand <= cumulative)
                return e;
        }

        return null;
    }
}
