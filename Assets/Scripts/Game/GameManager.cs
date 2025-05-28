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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }






}
