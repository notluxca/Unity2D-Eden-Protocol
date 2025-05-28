using TMPro;
using UnityEngine;
using DG.Tweening;

public class ScrapCountPannel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scrapText;
    private LootController lootController;
    private int previousLoot = -1;

    void Start()
    {
        scrapText.text = "0";
        lootController = FindFirstObjectByType<LootController>();
    }

    void Update()
    {
        int currentLoot = lootController.currentLoot;

        if (currentLoot != previousLoot)
        {
            scrapText.text = currentLoot.ToString();
            AnimatePanel();
            previousLoot = currentLoot;
        }
    }

    void AnimatePanel()
    {
        transform.DOKill(); // Stop previous tweens
        transform
            .DOScale(1.2f, 0.1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
                transform.DOScale(1f, 0.2f).SetEase(Ease.InOutQuad)
            );
    }
}
