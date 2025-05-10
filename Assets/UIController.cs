using UnityEngine;

public class UIController : MonoBehaviour
{

    public Animator fadeImage;


    public void FadeIn()
    {
        fadeImage.Play("FadeIn");
    }

    public void FadeOut()
    {
        fadeImage.Play("FadeOut");
    }

}
