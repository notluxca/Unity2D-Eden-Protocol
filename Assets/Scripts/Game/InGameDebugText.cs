using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameDebugText : MonoBehaviour
{
    private bool ShowDebugText = true;
    private DomeController domeController;
    private PlayerController playerController;
    private JetpackController jetpackController;
    private WaveController enemySpawner;
    private GunController gunController;
    private LootController lootController;
    private float o2Level;
    private float JetPackThrust;

    private void Start()
    {
        domeController = FindAnyObjectByType<DomeController>();
        playerController = FindAnyObjectByType<PlayerController>();
        gunController = FindAnyObjectByType<GunController>();
        enemySpawner = FindAnyObjectByType<WaveController>();
        lootController = FindAnyObjectByType<LootController>();
        jetpackController = FindAnyObjectByType<JetpackController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            //  SceneManager.LoadScene(4); //cheat scene 
        }
    }

    void OnGUI()
    {
        if (Input.GetKey(KeyCode.Tab))
        {

            GUI.Label(new Rect(300, 10, 300, 20), $"Gun Speed: {gunController.FireRate}");
            GUI.Label(new Rect(300, 25, 300, 20), $"Jetpack Thrust Level: {jetpackController.Thrust}");
            GUI.Label(new Rect(300, 40, 300, 20), $"Player Life: {playerController.life}");
            GUI.Label(new Rect(300, 55, 300, 20), $"Sucatas: {lootController.currentLoot}");
            GUI.Label(new Rect(300, 70, 300, 20), $"Dome Life: {domeController.currentLife}");
            GUI.Label(new Rect(300, 85, 300, 20), $"Alive Enemies: {enemySpawner.aliveEnemies.Count}");
        }
        // if (!ShowDebugText) return;
        // GUI.Label(new Rect(10, 10, 300, 20), $"O2 Level: {o2Level.ToString()}");

    }
}
