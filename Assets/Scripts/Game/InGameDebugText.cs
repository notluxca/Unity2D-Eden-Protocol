using UnityEngine;

public class InGameDebugText : MonoBehaviour
{
    private bool ShowDebugText = true;
    private DomeController domeController;
    private PlayerController playerController;
    private WaveController enemySpawner;
    private GunController gunController;
    private float o2Level;
    private float JetPackThrust;

    private void Start()
    {
        domeController = FindAnyObjectByType<DomeController>();
        playerController = FindAnyObjectByType<PlayerController>();
        gunController = FindAnyObjectByType<GunController>();
        enemySpawner = FindAnyObjectByType<WaveController>();
    }

    private void Update()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        o2Level = domeController.currentOxygen;
        JetPackThrust = playerController.thrust;
    }


    void OnGUI()
    {
        if (!ShowDebugText) return;
        GUI.Label(new Rect(10, 10, 300, 20), $"O2 Level: {o2Level.ToString()}");
        GUI.Label(new Rect(10, 25, 300, 20), $"Jetpack Thrust Level: {JetPackThrust.ToString()}");
        GUI.Label(new Rect(10, 40, 300, 20), $"Player Life: {playerController.life.ToString()}");
        // GUI.Label(new Rect(10, 55, 300, 20), $"CurrentWave: {enemySpawner.currentWaveName}");
    }
}
