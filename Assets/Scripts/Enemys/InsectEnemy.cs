using System;
using UnityEngine;

public class InsectEnemy : MonoBehaviour, IDamageable
{
    public Transform target;
    public float speed = 3f;
    public float variationAmount = 1f;
    public float variationSpeed = 5f;
    public float life = 1;
    public float AttackDamage = 1;

    public bool ShouldDropLoot = false;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector2 baseDirection;

    // Knockback control
    private float knockbackTimer = 0f;
    private float knockbackDuration = 0.2f;

    private Color originalColor;

    private void Start()
    {
        try
        {
            target = GameObject.FindWithTag("Player").transform;
        }
        catch (System.Exception)
        {
            target = GameObject.FindWithTag("Dome").transform;
            throw;
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalColor = spriteRenderer.color;

        PlayerController.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        target = GameObject.FindWithTag("Dome").transform;
    }

    private void Update()
    {
        if (target == null) return;

        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        Vector2 toTarget = (target.position - transform.position).normalized;
        baseDirection = toTarget;

        float offsetX = Mathf.PerlinNoise(Time.time * variationSpeed, 0f) - 0.5f;
        float offsetY = Mathf.PerlinNoise(0f, Time.time * variationSpeed) - 0.5f;
        Vector2 randomOffset = new Vector2(offsetX, offsetY) * variationAmount;

        Vector2 finalDirection = (baseDirection + randomOffset).normalized;

        rb.linearVelocity = finalDirection * speed;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = target.position.x < transform.position.x;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(AttackDamage, transform.position, 5);
            }
        }
    }

    public void Damage(float damage, Vector2 position, float knockbackForce)
    {
        life -= damage;

        StartCoroutine(FlashRed()); // <- Chama o efeito de flash

        Vector2 knockbackDirection = (Vector2)transform.position - position;
        knockbackDirection.Normalize();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            knockbackTimer = knockbackDuration;
        }

        if (life <= 0)
        {
            if (ShouldDropLoot)
            {
                Debug.Log("Dropped loot");
                // Instantiate(lootPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator FlashRed()
    {
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }

    private void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
