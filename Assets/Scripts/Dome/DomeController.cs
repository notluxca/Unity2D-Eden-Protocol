using UnityEngine;

public class DomeController : MonoBehaviour
{
    public float initialLife;
    public float currentLife;


    private bool shouldGenerateOxygen = true;
    public float initialOxygen = 0f;
    public float currentOxygen = 0f;
    public float O2GenerationRate = 0.1f; //default


    public void TakeDamage(float damage)
    {
        currentLife -= damage;
        if (currentLife <= 0)
        {
            // implode or break glass, gameover
        }
    }

    private void FixedUpdate()
    {
        if (shouldGenerateOxygen) currentOxygen += O2GenerationRate;
    }

    public void SetOxygenGeneration(bool value)
    {
        shouldGenerateOxygen = value;
    }


}
