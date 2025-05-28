using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifeCountPannel : MonoBehaviour
{
    public Image lifeImage;
    public float maxLife;
    public float currentLife;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        maxLife = playerController.life;
        currentLife = playerController.life;
        UpdateLifeUI();
    }

    private void Update()
    {
        if (Mathf.Abs(currentLife - playerController.life) > 0.01f)
        {
            UpdateLifeUI();
        }
    }

    void UpdateLifeUI()
    {
        currentLife = playerController.life;
        float percent = currentLife / maxLife;

        // Tween do fill
        lifeImage.DOFillAmount(percent, 0.3f).SetEase(Ease.OutQuad);

        // Animate the entire panel (this GameObject)
        transform.DOKill(); // Stop any ongoing scale tweens
        transform
            .DOScale(1.2f, 0.1f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
                transform.DOScale(1f, 0.2f).SetEase(Ease.InOutQuad)
            );
    }
}
