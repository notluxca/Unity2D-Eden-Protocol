using System;
using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    public Action OnDeath;

    void OnDestroy()
    {
        OnDeath?.Invoke();
    }
}
