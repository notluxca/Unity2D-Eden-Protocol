using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    AudioClip audioClip;
    public AudioMixer mixer;

    public void PlaySound(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    public void UpdateMusicVolume(Slider slider)
    {
        switch (slider.value)
        {
            case 5:
                mixer.SetFloat("MusicValue", -88f);
                break;
            case 4:
                mixer.SetFloat("MusicValue", -40f);
                break;
            case 3:
                mixer.SetFloat("MusicValue", -20f);
                break;
            case 2:
                mixer.SetFloat("MusicValue", -10f);
                break;
            case 1:
                mixer.SetFloat("MusicValue", 0f);
                break;
            case 0:
                mixer.SetFloat("MusicValue", 10f);
                break;
        }

        PlayerPrefs.SetFloat("MusicVolume", slider.value);
    }

    public void UpdateSfxVolume(Slider slider)
    {
        switch (slider.value)
        {
            case 5:
                mixer.SetFloat("SfxValue", -88f);
                break;
            case 4:
                mixer.SetFloat("SfxValue", -40f);
                break;
            case 3:
                mixer.SetFloat("SfxValue", -20f);
                break;
            case 2:
                mixer.SetFloat("SfxValue", -10f);
                break;
            case 1:
                mixer.SetFloat("SfxValue", 0f);
                break;
            case 0:
                mixer.SetFloat("SfxValue", 10f);
                break;
        }

        PlayerPrefs.SetFloat("SfxVolume", slider.value);
    }
}
