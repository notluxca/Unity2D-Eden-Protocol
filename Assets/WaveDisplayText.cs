using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaveDisplayText : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private WaveController waveController;
    private TextMeshProUGUI text;

    public float delayToShow = 2f;
    public float fadeDuration = 1f;
    public float displayTime = 2f;

    void Start()
    {
        waveController = FindAnyObjectByType<WaveController>();
        canvasGroup = GetComponent<CanvasGroup>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        WaveController.onWaveStart += HandleNewWave;
    }

    private void OnDisable()
    {
        WaveController.onWaveStart -= HandleNewWave;
    }

    private void HandleNewWave()
    {
        text.text = "Wave " + (waveController.currentWave + 1);
        ShowText();
    }

    private void ShowText()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, fadeDuration)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(displayTime, () =>
                {
                    canvasGroup.DOFade(0f, fadeDuration);

                });
            });
    }
}
