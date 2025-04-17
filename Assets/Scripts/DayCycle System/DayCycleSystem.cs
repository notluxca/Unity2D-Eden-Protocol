using System;
using System.Collections;
using UnityEngine;

public class DayCycleSystem : MonoBehaviour
{
    public static event Action OnNightStart;
    public static event Action OnNightEnd;

    public GameObject domeEnterTrigger;

    [Header("Setup")]
    [SerializeField] private float nightDuration = 10f;

    [Header("Animator")]
    [SerializeField] private Animator skyAnimator;
    [SerializeField] private string nightCycleName = "NightCycle";
    [SerializeField] private string dayIntroName = "DayIntro";
    [SerializeField] private string dayEndName = "DayEnd";

    private bool isNight = false;
    private bool isInDay = false;

    void Start()
    {
        isNight = false;
        isInDay = true;
        domeEnterTrigger.SetActive(false);
    }

    void OnEnable()
    {
        WaveController.onWaveEnd += EndNightManually;
    }

    void OnDisable()
    {
        WaveController.onWaveEnd -= EndNightManually;
    }

    public void EndNightManually()
    {
        // if (!isNight) return;
        // Debug.Log("Fim da noite chamado");

        isNight = false;
        OnNightEnd?.Invoke();
        PlayDayIntro();
    }

    [ContextMenu("Play Example Night")]
    public void PlayExampleNight()
    {
        PlayNight();
    }

    public void PlayNight()
    {
        domeEnterTrigger.SetActive(false);
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

        // yield return new WaitUntil(() => isInDay);
        OnNightEnd?.Invoke();
        // PlayDayIntro();
    }

    private void PlayDayIntro()
    {
        skyAnimator.speed = 1f;
        skyAnimator.Play(dayIntroName, 0, 0f);

        isInDay = true;
        Debug.Log("Amanhecendo... Dia começou");
        domeEnterTrigger.SetActive(true);
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

        PlayNight();
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


    public float GetNightDuration()
    {
        return nightDuration;
    }

}
