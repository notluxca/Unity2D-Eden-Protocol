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
        dayCycleSystem.StartDay(); // Começa de dia
        upgradePannel = FindFirstObjectByType<UpgradePannel>();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        WaveController.onWaveStart += () =>
        {
            Debug.Log("🔥 Wave " + waveController.currentWave + " iniciada.");
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
    }

    private void CheckIfNightIsOver()
    {
        if (allEnemiesDead && nightIsOver)
        {
            Debug.Log("✅ Noite encerrada e wave finalizada.");
            dayCycleSystem.StartDay();
            upgradePannel.OpenUpgradePannel();
            playerInDome = false;
        }
    }

    [ContextMenu("Proceed To Next Wave")]
    public void ProceedToNextWave()
    {
        if (dayCycleSystem.IsNight)
        {
            Debug.LogWarning("⚠️ Ainda é noite. Aguarde ela terminar.");
            return;
        }

        if (!dayCycleSystem.IsDay)
        {
            Debug.LogWarning("⚠️ Ainda não é dia. Espere a transição.");
            return;
        }

        Debug.Log("🚀 Iniciando próxima noite e wave");
        allEnemiesDead = false;
        nightIsOver = false;

        dayCycleSystem.ProceedToNight();
        waveController.StartNextWave();
    }

    public void SetPlayerInDome(bool value)
    {
        playerInDome = value;
    }
}
