using System;
using UnityEngine;

public class DomeController : MonoBehaviour, IDamageable
{
    public float initialLife = 8;
    public float currentLife;
    public GameObject Player;
    public UpgradePannel upgradeMenu;


    private bool shouldGenerateOxygen = true;
    public float initialOxygen = 0f;
    public float currentOxygen = 0f;
    public float O2GenerationRate = 0.1f; //default

    public static Action<int> OndomeHealthChange;


    public void TakeDamage(float damage)
    {

    }

    private void FixedUpdate()
    {
        // if (shouldGenerateOxygen) currentOxygen += O2GenerationRate;
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     DoDamage();
        // }
    }

    public void SetOxygenGeneration(bool value)
    {
        shouldGenerateOxygen = value;
    }

    public void EnterDome()
    {
        Player.SetActive(false);
        upgradeMenu.OpenUpgradePannel();
    }

    public void Damage(float damage, Vector2 position, float knockbackForce)
    {
        currentLife -= damage;
        Debug.Log("Domo atacado");
        if (currentLife <= 0)
        {
            // implode or break glass, gameover
        }
        OndomeHealthChange?.Invoke((int)currentLife);
    }

    [ContextMenu("Test Damage")]
    public void DoDamage()
    {
        ;
        Damage(1, Vector2.zero, 0);
    }


}
