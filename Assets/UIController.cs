using UnityEngine;

public class UIController : MonoBehaviour
{

    public GameObject PauseButton;
    public GameObject pauseMenu;
    public Animator fadeImage;
    public GameObject reticula;
    public AudioSource jetpackAudioSource;
    private GunController gunController;

    public void Start()
    {
        gunController = FindFirstObjectByType<GunController>();
    }

    public void FadeIn()
    {
        fadeImage.Play("FadeIn");
    }

    public void FadeOut()
    {
        fadeImage.Play("FadeOut");
    }


    public void PauseGame()
    {
        jetpackAudioSource.Stop();
        reticula.SetActive(false);
        PauseButton.SetActive(false);
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        gunController.ShootAlowed = true;
        jetpackAudioSource.Play();
        reticula.SetActive(true);
        PauseButton.SetActive(true);
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowRetitula()
    {
        Cursor.visible = false;
        reticula.SetActive(true);
    }

}
