using UnityEngine;
using TMPro;

public class UpgradePannel : MonoBehaviour
{

    public GameObject UpgradePannelCanvas;
    public TextMeshProUGUI oxygenText;

    private DomeController domeController;
    private PlayerController playerController;
    private GunController gunController;

    private void Awake()
    {
        domeController = GetComponentInParent<DomeController>();
        playerController = FindAnyObjectByType<PlayerController>();
        gunController = FindAnyObjectByType<GunController>();
    }

    private void Start()
    {
        UpgradePannelCanvas.SetActive(false);
    }


    [ContextMenu("Open Upgrade Pannel")]
    public void OpenUpgradePannel()
    {
        domeController.SetOxygenGeneration(false);
        UpgradePannelCanvas.SetActive(true);
        if (domeController != null) oxygenText.text = "Current Oxygen Level: " + domeController.currentOxygen.ToString();
    }

    [ContextMenu("Close Upgrade Pannel")]
    public void CloseUpgradePannel()
    {
        UpgradePannelCanvas.SetActive(false);
        domeController.SetOxygenGeneration(true);
    }


    public void UpgradePlayerSpeed()
    {

    }

    public void UpgradeDomeDurability()
    {

    }

    public void UpgradeGunFireRate()
    {

    }
}
