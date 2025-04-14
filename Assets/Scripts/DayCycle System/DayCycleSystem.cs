using System;
using System.Collections;
using UnityEngine;

public class DayCycleSystem : MonoBehaviour
{
    public static event Action OnNightStart;
    public static event Action OnNightEnd;

    [Header("Animator")]
    [SerializeField] private Animator skyAnimator;

    [Header("Animation Names")]
    [SerializeField] private string nightCycleName = "NightCycle";
    [SerializeField] private string dayIntroName = "DayIntro";
    [SerializeField] private string dayEndName = "DayEnd";

    private bool isNight = false;
    private bool isInDay = false;

    void Start()
    {
        isNight = false;
        isInDay = true;
    }

    void OnEnable()
    {
        EnemySpawner.onWaveEnd += EndNightManually;
    }

    void OnDisable()
    {
        EnemySpawner.onWaveEnd -= EndNightManually;
    }

    public void EndNightManually()
    {
        if (!isNight) return;

        isNight = false;
        OnNightEnd?.Invoke();
        PlayDayIntro();
    }

    [ContextMenu("Play Example Night")]
    public void PlayExampleNight()
    {
        PlayNight(10f);
    }

    public void PlayNight(float nightDuration)
    {
        if (isNight)
        {
            Debug.LogWarning("A noite já está em andamento.");
            return;
        }

        AnimationClip nightClip = GetAnimationClipByName(nightCycleName);
        if (nightClip == null)
        {
            Debug.LogError("Animação NightCycle não encontrada!");
            return;
        }

        float speed = nightClip.length / nightDuration;
        skyAnimator.speed = speed;
        skyAnimator.Play(nightCycleName, 0, 0f);

        isNight = true;
        isInDay = false;

        OnNightStart?.Invoke();
        StartCoroutine(NightRoutine(nightDuration));
    }

    private IEnumerator NightRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        isNight = false;

        OnNightEnd?.Invoke();
        PlayDayIntro();
    }

    private void PlayDayIntro()
    {
        skyAnimator.speed = 1f;
        skyAnimator.Play(dayIntroName, 0, 0f);

        isInDay = true;
        Debug.Log("Amanhecendo... Dia começou");
    }

    [ContextMenu("Proceed to Next Night")]
    public void ProceedToNextNight() // float nextNightDuration
    {
        if (!isInDay)
        {
            Debug.LogWarning("Ainda não é dia para iniciar a próxima noite.");
            return;
        }

        StartCoroutine(TransitionToNight(10f));
    }

    private IEnumerator TransitionToNight(float nightDuration)
    {
        isInDay = false;
        Debug.Log("Encerrando o dia...");

        skyAnimator.speed = 1f;
        skyAnimator.Play(dayEndName, 0, 0f);

        AnimationClip dayEndClip = GetAnimationClipByName(dayEndName);
        if (dayEndClip != null)
        {
            yield return new WaitForSeconds(dayEndClip.length);
        }

        PlayNight(nightDuration);
    }

    private AnimationClip GetAnimationClipByName(string clipName)
    {
        foreach (var clip in skyAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip;
        }
        return null;
    }
}
