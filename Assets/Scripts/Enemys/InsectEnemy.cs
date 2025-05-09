using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class InsectEnemy : MonoBehaviour, IDamageable
{
    public Transform target;
    public float speed = 3f;
    public float variationAmount = 1f;
    public float variationSpeed = 5f;
    public float life = 1;
    public float AttackDamage = 1;
    public bool ShouldDropLoot = false;

    private EnemyMovement movement;
    private EnemyDamageHandler damageHandler;

    private void Start()
    {
        if (target == null)
        {
            var player = GameObject.FindWithTag("Player");
            target = player != null ? player.transform : GameObject.FindWithTag("Dome").transform;
        }


        movement = GetComponent<EnemyMovement>();
        damageHandler = GetComponent<EnemyDamageHandler>();
        // movement = new EnemyMovement(GetComponent<SpriteRenderer>(), this);

        // damageHandler = new EnemyDamageHandler(GetComponent<Rigidbody2D>(), GetComponent<SpriteRenderer>(), this);
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
        if (target != null)
            movement.MoveTowards(target);
        movement.Tick(target);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        IDamageable damageable;
        if (other.gameObject.TryGetComponent<IDamageable>(out damageable))
            if (damageable != null) damageable?.Damage(AttackDamage, transform.position, 5);
    }

    public void Damage(float damage, Vector2 position, float knockbackForce)
    {
        damageHandler.ApplyDamage(damage, position, knockbackForce);
    }
}
