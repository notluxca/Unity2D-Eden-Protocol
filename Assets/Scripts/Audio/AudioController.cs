using UnityEngine;

public class AudioController : MonoBehaviour
{
    AudioClip audioClip;

    public void PlaySound(AudioClip clip)
    {
        GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
