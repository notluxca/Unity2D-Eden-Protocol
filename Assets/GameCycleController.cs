using System;
using UnityEngine;

// se inscrever no evento do WaveController que diz se o ultimo inimigo da current wave for morto
// liberar entrada no domo
// após upgrade do domo, prosseguir para a proxima noite e tirar o player do domo.
public class GameCycleController : MonoBehaviour
{
    [SerializeField] private WaveController WaveController;
    [SerializeField] private DayCycleSystem DayCycleSystem;

    // wave end info
    [SerializeField] private bool PlayerInDome;
    [SerializeField] private bool AllEnemysDead;
    [SerializeField] private bool NightOver;

    public bool GetPlayerInDome() { return PlayerInDome; }
    public bool GetAllEnemysDead() { return AllEnemysDead; }
    public bool GetNightOver() { return NightOver; }

    private void Start()
    {
        StartGame();
        WaveController.onWaveStart += DisplayNewWaveText;
    }

    private void DisplayNewWaveText()
    {
        Debug.Log("Wave " + WaveController.currentWave + 1);
    }

    [ContextMenu("Start Game")]
    private void StartGame()
    {
        DayCycleSystem.PlayNight();
        WaveController.StartNextWave();
    }

    // Should cycle the day to night at same time
    public void ProceedToNextWave()
    {
        if (!(GetAllEnemysDead() && GetNightOver())) return;

        DayCycleSystem.ProceedToNextNight();
        WaveController.StartNextWave();
    }




}
