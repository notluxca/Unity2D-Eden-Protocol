using UnityEngine;

public class InsectEnemy : MonoBehaviour, IDamageable
{
    public Transform target;
    public float speed = 3f;
    public float variationAmount = 1f;
    public float variationSpeed = 5f;
    public float life = 1; // 1 default
    public float AttackDamage = 1;

    public SpriteRenderer spriteRenderer;

    private Vector2 baseDirection;

    public float current_life => throw new System.NotImplementedException();

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (target == null) return;

        // Direção base ao player
        Vector2 toTarget = (target.position - transform.position).normalized;
        baseDirection = toTarget;

        // Variação tipo inseto
        float offsetX = Mathf.PerlinNoise(Time.time * variationSpeed, 0f) - 0.5f;
        float offsetY = Mathf.PerlinNoise(0f, Time.time * variationSpeed) - 0.5f;
        Vector2 randomOffset = new Vector2(offsetX, offsetY) * variationAmount;

        Vector2 finalDirection = (baseDirection + randomOffset).normalized;

        GetComponent<Rigidbody2D>().linearVelocity = finalDirection * speed;

        // Flipar sprite com base na direção do player
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = target.position.x < transform.position.x;
        }

        // (Opcional) Rotacionar corpo pra direção de movimento
        // RotateTowards(finalDirection);
    }

    private void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamageable>().Damage(AttackDamage);
        }

    }


    public void Damage(float damage)
    {
        life -= damage;
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
