using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveController : MonoBehaviour
{
    [Header("Waves")]
    public List<WaveData> waves;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [SerializeField] private DayCycleSystem dayCyleSystem; // Script externo que define a duração da noite

    public int WaveDropCount = 0;

    private int currentWaveIndex = -1;
    public int currentWave => currentWaveIndex;
    private bool waveInProgress = false;
    private Coroutine currentWaveCoroutine;
    private GameManager gameManager;
    private bool PlayerDead = false;

    public List<GameObject> aliveEnemies = new List<GameObject>();

    public static Action onWaveStart, onWaveEnd;

    private void Start()
    {
        PlayerController.OnPlayerDeath += OnPlayerDeath;
        gameManager = FindAnyObjectByType<GameManager>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        gameManager = FindAnyObjectByType<GameManager>();
        StopAllCoroutines();
    }

    private void OnPlayerDeath()
    {
        PlayerDead = true;
    }

    public void StartNextWave()
    {
        if (waveInProgress || currentWaveIndex + 1 >= waves.Count) // Já existe uma wave em andamento ou não há mais waves.
        {
            if (currentWaveIndex + 1 >= waves.Count)
            {
                gameManager.Win();
            }

            return;
        }

        currentWaveIndex++;
        var wave = waves[currentWaveIndex];

        Debug.Log($"Iniciando Wave {currentWaveIndex + 1}: {wave.waveName}");

        waveInProgress = true;
        onWaveStart?.Invoke();

        currentWaveCoroutine = StartCoroutine(SpawnWaveRoutine(wave));
    }

    IEnumerator SpawnWaveRoutine(WaveData wave)
    {
        float nightDuration = dayCyleSystem.GetNightDuration(); // duração da noite
        float delayBetweenSpawns = nightDuration / wave.enemyCount;

        // Seleciona quais inimigos vão drop coins
        WaveDropCount = UnityEngine.Random.Range(1, 6);
        HashSet<int> dropCoinIndexes = SelectRandomIndexes(wave.enemyCount, WaveDropCount);

        for (int i = 0; i < wave.enemyCount; i++)
        {
            bool shouldDropCoin = dropCoinIndexes.Contains(i);
            SpawnEnemyFromWaveData(wave, shouldDropCoin);
            yield return new WaitForSeconds(delayBetweenSpawns);
        }

        yield return new WaitUntil(() => aliveEnemies.Count == 0);

        Debug.Log($"Wave {currentWaveIndex + 1} concluida.");

        waveInProgress = false;
        onWaveEnd?.Invoke();
    }

    void SpawnEnemyFromWaveData(WaveData wave, bool shouldDropCoin)
    {
        EnemyData enemyData = GetRandomEnemy(wave.enemies);
        if (enemyData == null) return; // Nenhum inimigo foi selecionado

        Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyData.enemyPrefab, spawnPoint.position, Quaternion.identity);
        InsectEnemy insectEnemy = enemy.GetComponent<InsectEnemy>();
        insectEnemy.ShouldDropLoot = shouldDropCoin;
        insectEnemy.target = UnityEngine.Random.Range(0, 2) == 0 ? GameObject.FindWithTag("Dome").transform : GameObject.FindWithTag("Player").transform; // Decide entre domo e player

        aliveEnemies.Add(enemy);
        EnemyDeathHandler handler = enemy.GetComponent<EnemyDeathHandler>(); //Todo: Mudar para InsectEnemy
        handler.OnDeath += () => aliveEnemies.Remove(enemy);

        if (PlayerDead)
        {
            insectEnemy.AttackDamage = 5;
            insectEnemy.target = GameObject.FindWithTag("Dome").transform;
        }
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
