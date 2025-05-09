using UnityEngine;
using System.Collections;

public class EnemyDamageHandler : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private InsectEnemy enemy;
    private Color originalColor;
    public GameObject lootPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponent<InsectEnemy>();
        originalColor = spriteRenderer.color;
    }

    public void ApplyDamage(float damage, Vector2 hitPosition, float knockbackForce)
    {
        enemy.life -= damage;
        enemy.StartCoroutine(FlashRed());

        if (knockbackForce > 0)
            enemy.StartCoroutine(ApplyKnockback(hitPosition, knockbackForce));

        if (enemy.life <= 0)
        {
            if (enemy.ShouldDropLoot)
            {
                Debug.Log("Dropped loot");
                Object.Instantiate(lootPrefab, enemy.transform.position, Quaternion.identity);
            }

            Object.Destroy(enemy.gameObject);
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    private IEnumerator ApplyKnockback(Vector2 hitPosition, float knockbackForce)
    {
        Vector2 knockbackDir = (Vector2)enemy.transform.position - hitPosition;
        knockbackDir.Normalize();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(0.1f);

        rb.linearVelocity = Vector2.zero; // reset knockbackForce
    }
}