using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public void StartGameplay()
    {
        SceneManager.LoadScene(2); // 2 = gameplay Scene
    }
}
