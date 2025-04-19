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

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (target == null) return;

        // Se estiver em knockback, não se move
        if (knockbackTimer > 0)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        // Direção base ao player
        Vector2 toTarget = (target.position - transform.position).normalized;
        baseDirection = toTarget;

        // Variação tipo inseto
        float offsetX = Mathf.PerlinNoise(Time.time * variationSpeed, 0f) - 0.5f;
        float offsetY = Mathf.PerlinNoise(0f, Time.time * variationSpeed) - 0.5f;
        Vector2 randomOffset = new Vector2(offsetX, offsetY) * variationAmount;

        Vector2 finalDirection = (baseDirection + randomOffset).normalized;

        rb.linearVelocity = finalDirection * speed;

        // Flipar sprite com base na direção do player
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = target.position.x < transform.position.x;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            float knockbackForce = 2;
            var damageable = other.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.Damage(AttackDamage, transform.position, knockbackForce);
            }
        }
    }

    public void Damage(float damage, Vector2 position, float knockbackForce)
    {
        life -= damage;

        Vector2 knockbackDirection = (Vector2)transform.position - position;
        knockbackDirection.Normalize();

        if (rb != null)
        {
            Debug.Log("Knockback applied");
            rb.linearVelocity = Vector2.zero; // Zera antes do knockback
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            knockbackTimer = knockbackDuration; // Pausa o movimento normal
        }

        if (life <= 0)
        {
            if (ShouldDropLoot)
            {
                Debug.Log("Dropped loot");
                // Instantiate(lootPrefab, transform.position, Quaternion.identity); // opcional
            }

            Destroy(gameObject);
        }
    }

    // (Opcional) Rotacionar corpo pra direção de movimento
    private void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
