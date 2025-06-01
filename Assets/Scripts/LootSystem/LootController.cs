using UnityEngine;

public class LootController : MonoBehaviour
{
    public int currentLoot;
    public AudioSource audioSource;
    public AudioClip collectAudioClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public bool TrySpeendLoot(int amount)
    {
        if (currentLoot >= amount)
        {
            currentLoot -= amount;
            return true;
        }
        return false;
    }

    public void AddLoot(int amount)
    {
        audioSource.PlayOneShot(collectAudioClip);
        currentLoot += amount;
    }


    public int GetValue()
    {
        return currentLoot;
    }


}
