using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject MainMenuScreen;
    public GameObject creditsScreen;
    public Animator fadeImage;

    private void Start()
    {
        MainMenuScreen.SetActive(true);
        creditsScreen.SetActive(false);
    }

    public void StartGame()
    {
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
        MainMenuScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void CloseCredits()
    {
        MainMenuScreen.SetActive(true);
        creditsScreen.SetActive(false);
    }


}
