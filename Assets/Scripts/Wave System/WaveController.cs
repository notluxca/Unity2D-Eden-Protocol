using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [Header("Waves")]
    public List<WaveData> waves;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [SerializeField] private DayCycleSystem dayCyleSystem; // Script externo que define a duração da noite

    public int WaveDropCount = 0;

    private int currentWaveIndex = -1;
    private bool waveInProgress = false;
    private Coroutine currentWaveCoroutine;

    public List<GameObject> aliveEnemies = new List<GameObject>();

    public static Action onWaveStart, onWaveEnd;

    public void StartNextWave()
    {
        if (waveInProgress || currentWaveIndex + 1 >= waves.Count)
        {
            Debug.LogWarning("⚠️ Já existe uma wave em andamento ou não há mais waves.");
            return;
        }

        currentWaveIndex++;
        var wave = waves[currentWaveIndex];

        Debug.Log($"🌙 Iniciando Wave {currentWaveIndex + 1}: {wave.waveName}");

        waveInProgress = true;
        onWaveStart?.Invoke();

        currentWaveCoroutine = StartCoroutine(SpawnWaveRoutine(wave));
    }

    IEnumerator SpawnWaveRoutine(WaveData wave)
    {
        Debug.Log("Começando wave");
        float nightDuration = dayCyleSystem.GetNightDuration(); // duração da noite
        float delayBetweenSpawns = nightDuration / wave.enemyCount;

        // Seleciona quais inimigos vão drop coins
        WaveDropCount = UnityEngine.Random.Range(1, 4);
        HashSet<int> dropCoinIndexes = SelectRandomIndexes(wave.enemyCount, WaveDropCount);

        for (int i = 0; i < wave.enemyCount; i++)
        {
            bool shouldDropCoin = dropCoinIndexes.Contains(i);
            SpawnEnemyFromWaveData(wave, shouldDropCoin);
            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        yield return new WaitUntil(() => aliveEnemies.Count == 0);

        Debug.Log($"✅ Wave {currentWaveIndex + 1} finalizada.");

        waveInProgress = false;
        onWaveEnd?.Invoke();
    }

    void SpawnEnemyFromWaveData(WaveData wave, bool shouldDropCoin)
    {
        EnemyData enemyData = GetRandomEnemy(wave.enemies);
        if (enemyData == null)
        {
            Debug.LogWarning("Nenhum inimigo foi selecionado!");
            return;
        }

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPoint.position, Quaternion.identity);


        enemy.GetComponent<InsectEnemy>().ShouldDropLoot = shouldDropCoin;

        aliveEnemies.Add(enemy);

        EnemyDeathHandler handler = enemy.AddComponent<EnemyDeathHandler>();
        handler.OnDeath += () => aliveEnemies.Remove(enemy);
    }

    EnemyData GetRandomEnemy(List<EnemySpawnData> enemyPool)
    {
        float totalChance = 0f;
        foreach (var e in enemyPool)
            totalChance += e.spawnChance;

        float rand = UnityEngine.Random.Range(0f, totalChance);
        float cumulative = 0f;

        foreach (var e in enemyPool)
        {
            cumulative += e.spawnChance;
            if (rand <= cumulative)
                return e.enemyData;
        }

        return null;
    }

    HashSet<int> SelectRandomIndexes(int total, int count)
    {
        HashSet<int> selectedIndexes = new HashSet<int>();
        while (selectedIndexes.Count < count)
        {
            int randIndex = UnityEngine.Random.Range(0, total);
            selectedIndexes.Add(randIndex);
        }
        return selectedIndexes;
    }
}
