using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSceneController : MonoBehaviour
{

    public Animator FadeImage;

    public void EndCutscene()
    {
        StartCoroutine(EndCutsceneRoutine());
    }

    IEnumerator EndCutsceneRoutine()
    {
        FadeImage.Play("FadeIn");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }
}
