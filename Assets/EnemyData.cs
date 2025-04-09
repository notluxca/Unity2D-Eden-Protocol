using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    public GameObject enemyPrefab;
    [Range(0f, 1f)]
    public float spawnChance; // chance de spawn dentro da wave
}
