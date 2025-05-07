using System;
using System.Collections;
using UnityEngine;

public class DayCycleSystem : MonoBehaviour
{
    public static event Action OnNightStart;
    public static event Action OnNightEnd;

    [Header("Setup")]
    [SerializeField] private float nightDuration = 10f;

    [Header("Animator")]
    [SerializeField] private Animator skyAnimator;
    [SerializeField] private string dayCycleName = "DayCycle";
    [SerializeField] private string nightCycleName = "NightCycle";

    private bool isNight = false;

    public bool IsNight => isNight;
    public bool IsDay => !isNight;

    // 1. Executar o ciclo de dia até o final
    public void PlayDayCycle()
    {
        isNight = false;
        skyAnimator.speed = 1f;
        skyAnimator.Play(dayCycleName, 0, 0f);
    }

    // 2. Iniciar o ciclo de noite com transição
    public void StartNightCycle()
    {
        if (isNight) return;
        StartCoroutine(FinishDayThenStartNight());
    }

    // 3. Corrotina para finalizar rapidamente o dia antes da noite
    private IEnumerator FinishDayThenStartNight()
    {
        AnimationClip dayClip = GetAnimationClipByName(dayCycleName);
        if (dayClip == null)
        {
            Debug.LogError("Animação DayCycle não encontrada.");
            yield break;
        }

        // Finalizar o dia rapidamente
        float fastSpeed = 4f; // ou use: dayClip.length / 1f;
        skyAnimator.speed = fastSpeed;
        skyAnimator.Play(dayCycleName, 0, 0.5f); // começa da metade, opcional

        yield return new WaitForSeconds(dayClip.length / fastSpeed);

        // Agora iniciar a noite
        AnimationClip nightClip = GetAnimationClipByName(nightCycleName);
        if (nightClip == null)
        {
            Debug.LogError("Animação NightCycle não encontrada.");
            yield break;
        }

        isNight = true;
        float nightSpeed = nightClip.length / nightDuration;
        skyAnimator.speed = nightSpeed;
        skyAnimator.Play(nightCycleName, 0, 0f);

        OnNightStart?.Invoke();
        StartCoroutine(NightDurationRoutine());
    }

    private IEnumerator NightDurationRoutine()
    {
        yield return new WaitForSeconds(nightDuration);
        isNight = false;
        OnNightEnd?.Invoke();
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

    public float GetNightDuration() => nightDuration;
}
