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
    [SerializeField] private string dayCycle = "DayCycle";
    [SerializeField] private string nightCycleName = "NightCycle";
    [SerializeField] private string dayEndName = "DayEnd";
    [SerializeField] private string dayIntroName = "DayIntro";

    [SerializeField] private GameObject domeEnterTrigger;

    private bool isNight = false;
    private bool isInDay = false;

    public bool IsNight => isNight;
    public bool IsDay => isInDay;

    private void Start()
    {
        StartDay();
    }

    public void StartDay()
    {
        isNight = false;
        isInDay = true;

        skyAnimator.speed = 1f;
        skyAnimator.Play(dayCycle, 0, 0f);
        domeEnterTrigger.SetActive(true);

        // Debug.Log("Dia começou");
    }

    public void ProceedToNight()
    {
        if (!isInDay)
        {
            // Debug.LogWarning(" Já é noite.");
            return;
        }

        StartCoroutine(TransitionToNight());
    }

    private IEnumerator TransitionToNight()
    {
        isInDay = false;
        domeEnterTrigger.SetActive(false);
        // Debug.Log("Transição para noite...");

        skyAnimator.speed = 1f;
        skyAnimator.Play(dayEndName, 0, 0f);

        AnimationClip endClip = GetAnimationClipByName(dayEndName);
        if (endClip != null)
            yield return new WaitForSeconds(endClip.length);

        PlayNight();
    }

    private void PlayNight()
    {
        AnimationClip nightClip = GetAnimationClipByName(nightCycleName);
        if (nightClip == null)
        {
            // Debug.LogError("Animação NightCycle não encontrada!");
            return;
        }

        float speed = nightClip.length / nightDuration;
        skyAnimator.speed = speed;
        skyAnimator.Play(nightCycleName, 0, 0f);

        isNight = true;
        // Debug.Log("Noite começou");

        OnNightStart?.Invoke();
        StartCoroutine(NightRoutine());
    }

    private IEnumerator NightRoutine()
    {
        yield return new WaitForSeconds(nightDuration);
        EndNight();
    }

    private void EndNight()
    {
        if (!isNight) return;

        isNight = false;
        // Debug.Log("Noite terminou");
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
