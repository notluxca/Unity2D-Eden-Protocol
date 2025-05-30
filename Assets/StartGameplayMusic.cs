using UnityEngine;

public class StartGameplayMusic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BackgroundMusicPlayer.RequestMusicChange(BackgroundMusicPlayer.MusicType.Gameplay);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
