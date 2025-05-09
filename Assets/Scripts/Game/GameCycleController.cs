using UnityEngine;

public class GameCycleController : MonoBehaviour
{
    [SerializeField] private WaveController waveController;
    [SerializeField] private DayCycleSystem dayCycleSystem;

    private bool playerInDome = false;
    private bool allEnemiesDead = false;
    private bool nightIsOver = false;
    private UpgradePannel upgradePannel;

    private void Start()
    {
        // dayCycleSystem.PlayDayCycle(); // Começa de dia
        upgradePannel = FindFirstObjectByType<UpgradePannel>();
        SubscribeToEvents();
        ProceedToNextWave();
    }

    private void SubscribeToEvents()
    {
        WaveController.onWaveStart += () =>
        {
            Debug.Log("Wave " + waveController.currentWave + " iniciada.");
        };

        WaveController.onWaveEnd += () =>
        {
            allEnemiesDead = true;
            CheckIfNightIsOver();
        };

        DayCycleSystem.OnNightEnd += () =>
        {
            nightIsOver = true;
            CheckIfNightIsOver();
        };
        DayCycleSystem.OnNightStart += () =>
        {
            waveController.StartNextWave();
        };
    }

    private void CheckIfNightIsOver()
    {
        if (allEnemiesDead && nightIsOver)
        {
            Debug.Log("Noite encerrada e wave finalizada.");
            dayCycleSystem.PlayDayCycle();
            upgradePannel.OpenUpgradePannel();
            playerInDome = false;
        }
    }

    [ContextMenu("Proceed To Next Wave")]
    public void ProceedToNextWave()
    {
        if (!dayCycleSystem.IsDay) return;

        Debug.Log("Iniciando próxima wave...");
        allEnemiesDead = false;
        nightIsOver = false;

        dayCycleSystem.StartNightCycle();
        // subscribe to nitght start event

    }

    public void SetPlayerInDome(bool value)
    {
        playerInDome = value;
    }
}
