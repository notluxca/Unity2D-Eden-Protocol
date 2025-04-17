using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class JetpackAudioController : MonoBehaviour
{
    public PlayerController playerController; // Referência ao PlayerController
    public float fadeSpeed = 2f; // Velocidade de fade in/out
    public float maxVolume = 1f; // Volume máximo do áudio

    private AudioSource jetpackAudioSource;
    private bool wasThrustingLastFrame = false;

    void Start()
    {
        jetpackAudioSource = GetComponent<AudioSource>();
        jetpackAudioSource.volume = 0f;
        jetpackAudioSource.loop = true;
        jetpackAudioSource.Play(); // Toca desde o início, mas começa silencioso
    }

    void Update()
    {
        if (playerController == null) return;

        // Verifica se o jogador está usando o jetpack
        bool isThrusting = !playerController.Grounded &&
                           (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));

        // Faz fade in se estiver usando o jetpack
        if (isThrusting)
        {
            jetpackAudioSource.volume = Mathf.MoveTowards(jetpackAudioSource.volume, maxVolume, Time.deltaTime * fadeSpeed);
        }
        else
        {
            jetpackAudioSource.volume = Mathf.MoveTowards(jetpackAudioSource.volume, 0f, Time.deltaTime * fadeSpeed);
        }

        wasThrustingLastFrame = isThrusting;
    }
}
