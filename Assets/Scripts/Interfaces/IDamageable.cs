using UnityEngine;

public interface IDamageable
{
    void Damage(float damage, Vector2 position, float knockbackForce);
}
