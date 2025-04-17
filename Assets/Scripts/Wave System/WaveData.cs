using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class WaveData
{
    public string waveName;
    public float spawnDelay = 1f;
    public int enemyCount = 10;

    public List<EnemySpawnData> enemies;
}

[System.Serializable]
public class EnemySpawnData
{
    public EnemyData enemyData;
    [Range(0, 1)] public float spawnChance = 1f; // chance relativa nesta wave
}
