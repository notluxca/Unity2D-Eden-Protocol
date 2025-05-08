using UnityEngine;
using TMPro;

public class UpgradePannel : MonoBehaviour
{

    public GameObject UpgradePannelCanvas;
    public TextMeshProUGUI oxygenText;

    private DomeController domeController;
    private PlayerController playerController;
    private GunController gunController;
    private LootController lootController;
    private GameCycleController gameCycleController;

    private void Awake()
    {
        domeController = GetComponentInParent<DomeController>();
        playerController = FindAnyObjectByType<PlayerController>();
        gunController = FindAnyObjectByType<GunController>();
        lootController = FindAnyObjectByType<LootController>();
        gameCycleController = FindAnyObjectByType<GameCycleController>();
    }

    private void Start()
    {
        UpgradePannelCanvas.SetActive(false);
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.J)) // Cheats
        // {
        //     UpgradeGunFireRate();
        //     UpgradePlayerSpeed();
        // }
    }

    public void OpenUpgradePannel()
    {
        domeController.SetOxygenGeneration(false);
        UpgradePannelCanvas.SetActive(true);
        if (domeController != null) oxygenText.text = "Current Oxygen Level: " + domeController.currentOxygen.ToString();
    }
    public void CloseUpgradePannel()
    {
        UpgradePannelCanvas.SetActive(false);
        domeController.SetOxygenGeneration(true);
    }


    public void UpgradePlayerSpeed()
    {
        if (!lootController.TrySpeendLoot(1)) return; // se a compra falhar retorne
        Upgrade();
        playerController.thrust += 2f;
        playerController.speed += 0.4f;
    }

    public void UpgradeDomeDurability()
    {
        if (!lootController.TrySpeendLoot(1)) return; // se a compra falhar retorne
        Upgrade();
        Debug.Log("Dome durability on");
    }

    public void UpgradeGunFireRate()
    {
        if (!lootController.TrySpeendLoot(1)) return; // se a compra falhar retorne
        Upgrade();
        gunController.FireRate += 0.6f;
    }

    private void Upgrade()
    {
        if (lootController.GetValue() <= 0)
        {
            CloseUpgradePannel();
            gameCycleController.ProceedToNextWave();
        }
    }
}
