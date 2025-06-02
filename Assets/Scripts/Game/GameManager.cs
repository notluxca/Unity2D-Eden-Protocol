using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    UIController uiController;



    void Awake()
    {
        uiController = FindFirstObjectByType<UIController>();
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += PlayerDeath;
    }

    public void PlayerDeath()
    {
        StartCoroutine(PlayerDeathSequence());
    }

    IEnumerator PlayerDeathSequence()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 1f;


    }

    public void GameOver()
    {
        // call transition
        // restart scene
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        uiController.FadeIn();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    public void Win()
    {
        Debug.Log("Win");
        StartCoroutine(WinSequence());
    }

    IEnumerator WinSequence()
    {
        uiController.FadeIn();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(3); // Scene 3 is Victory scene that after return to main menu
    }






}
