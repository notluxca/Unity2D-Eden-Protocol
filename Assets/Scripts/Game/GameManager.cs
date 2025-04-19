using UnityEngine;

public class GameManager : MonoBehaviour
{
    DayCycleSystem dayCycleSystem;

    void Start()
    {
        dayCycleSystem = FindFirstObjectByType<DayCycleSystem>();
        // dayCycleSystem.PlayExampleNight();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
