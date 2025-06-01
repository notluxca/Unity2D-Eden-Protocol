using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenuScreen;
    public GameObject creditsScreen;
    public Animator fadeImage;
    private AudioSource audioSource;
    public AudioClip uiclickSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        MainMenuScreen.SetActive(true);
        creditsScreen.SetActive(false);
        Cursor.visible = true; // Liga a vibilidade do mouse
        BackgroundMusicPlayer.RequestMusicChange(BackgroundMusicPlayer.MusicType.Menu);
    }

    public void StartGame()
    {
        audioSource.PlayOneShot(uiclickSound);
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        fadeImage.Play("FadeIn");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1); // get cutscene index
    }

    public void OpenCredits()
    {
        audioSource.PlayOneShot(uiclickSound);
        MainMenuScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void CloseCredits()
    {
        audioSource.PlayOneShot(uiclickSound);
        MainMenuScreen.SetActive(true);
        creditsScreen.SetActive(false);
    }


}
