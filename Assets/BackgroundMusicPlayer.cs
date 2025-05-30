using UnityEngine;
using System;
using System.Collections;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public static event Action<MusicType> OnMusicChangeRequested;

    public enum MusicType { Menu, Gameplay }

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameplayMusic;
    [SerializeField] private float fadeDuration = 1.5f;

    private static BackgroundMusicPlayer instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = 0f;

        OnMusicChangeRequested += HandleMusicChange;
    }

    private void Start()
    {
        // Start with menu music by default
        ChangeMusic(menuMusic);
    }

    private void OnDestroy()
    {
        OnMusicChangeRequested -= HandleMusicChange;
    }

    private void HandleMusicChange(MusicType type)
    {
        AudioClip targetClip = type == MusicType.Menu ? menuMusic : gameplayMusic;
        if (audioSource.clip == targetClip) return;

        StartCoroutine(FadeAndSwitchMusic(targetClip));
    }

    private IEnumerator FadeAndSwitchMusic(AudioClip newClip)
    {
        // Fade out
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        while (audioSource.volume < 1)
        {
            audioSource.volume += Time.unscaledDeltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = 1f;
    }

    private void ChangeMusic(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 1f;
        audioSource.Play();
    }

    // Static method to call from anywhere
    public static void RequestMusicChange(MusicType type)
    {
        OnMusicChangeRequested?.Invoke(type);
    }
}
