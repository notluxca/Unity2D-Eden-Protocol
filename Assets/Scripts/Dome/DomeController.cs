using System;
using System.Collections;
using UnityEngine;

public class DomeController : MonoBehaviour, IDamageable
{
    public float initialLife;
    public float currentLife;
    public float damageCooldown;
    public GameObject Player;
    public UpgradePannel upgradeMenu;

    AudioSource audioSource;
    [SerializeField] private AudioClip damageClip;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentLife = initialLife;
    }


    private bool canTakeDamage = true;

    public static Action<int> OndomeHealthChange;

    public void EnterDome()
    {
        Player.SetActive(false);
        upgradeMenu.OpenUpgradePannel();
    }

    public void Damage(float damage, Vector2 position, float knockbackForce)
    {
        if (canTakeDamage == false) return;
        StartCoroutine(DamageCoroutine(damage));
    }

    IEnumerator DamageCoroutine(float damage) // to implement damage cooldown
    {
        audioSource.PlayOneShot(damageClip);
        currentLife -= damage;
        if (currentLife <= 0)
        {
            // implode or break glass, gameover
        }
        OndomeHealthChange?.Invoke((int)currentLife);
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}
