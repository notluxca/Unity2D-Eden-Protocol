using UnityEngine;
using TMPro;

public class UpgradePannel : MonoBehaviour
{

    public GameObject UpgradePannelCanvas;
    public TextMeshProUGUI oxygenText;

    private DomeController domeController;
    private PlayerController playerController;
    private JetpackController jetpackController;
    private GunController gunController;
    private LootController lootController;
    private GameCycleController gameCycleController;

    private AudioSource audioSource;
    [SerializeField] private AudioClip openPannelSound;
    [SerializeField] private AudioClip clickSound;

    private void Awake()
    {
        domeController = GetComponentInParent<DomeController>();
        playerController = FindAnyObjectByType<PlayerController>();
        gunController = FindAnyObjectByType<GunController>();
        lootController = FindAnyObjectByType<LootController>();
        gameCycleController = FindAnyObjectByType<GameCycleController>();
        jetpackController = FindAnyObjectByType<JetpackController>();
        audioSource = GetComponent<AudioSource>();
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
        audioSource.PlayOneShot(openPannelSound);
        UpgradePannelCanvas.SetActive(true);
    }
    public void CloseUpgradePannel()
    {
        audioSource.PlayOneShot(openPannelSound);
        UpgradePannelCanvas.SetActive(false);
    }


    public void UpgradePlayerSpeed()
    {
        if (!lootController.TrySpeendLoot(1)) return; // se a compra falhar retorne
        Upgrade();
        jetpackController.Thrust += 0.5f;
        jetpackController.Speed += 0.2f;
    }

    public void UpgradeDomeDurability()
    {
        if (!lootController.TrySpeendLoot(1)) return; // se a compra falhar retorne
        Upgrade();
        domeController.RepairDome(5);
    }

    public void UpgradeGunFireRate()
    {
        if (!lootController.TrySpeendLoot(1)) return; // se a compra falhar retorne
        Upgrade();
        gunController.FireRate += 0.9f;
    }

    private void Upgrade()
    {
        audioSource.PlayOneShot(clickSound);
        if (lootController.GetValue() <= 0)
        {
            CloseUpgradePannel();
            gameCycleController.ProceedToNextWave();
        }
    }
}
