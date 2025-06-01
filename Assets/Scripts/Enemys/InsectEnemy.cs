using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(AudioSource))]
public class InsectEnemy : MonoBehaviour, IDamageable
{
    public Transform target;
    public float speed = 3f;
    public float variationAmount = 1f;
    public float variationSpeed = 5f;
    public float life = 1;
    public float AttackDamage = 1;
    public bool ShouldDropLoot = false;
    public AudioClip[] hurtClips;   // Sons de quando leva dano mas ainda está vivo
    // public AudioClip[] damageClips; // Sons de quando morre
    public AudioClip attackClip;

    private AudioSource audioSource;
    private EnemyMovement movement;
    private EnemyDamageHandler damageHandler;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider;

    private void Start()
    {
        if (target == null)
        {
            var player = GameObject.FindWithTag("Player");
            target = player != null ? player.transform : GameObject.FindWithTag("Dome").transform;
        }

        movement = GetComponent<EnemyMovement>();
        damageHandler = GetComponent<EnemyDamageHandler>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        PlayerController.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        AttackDamage = 5;
        target = GameObject.FindWithTag("Dome").transform;
    }

    private void Update()
    {
        if (isDead) return;
        if (target != null)
            movement.MoveTowards(target);
        movement.Tick(target);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            audioSource.PlayOneShot(attackClip);
            damageable?.Damage(AttackDamage, transform.position, 5);
        }
    }

    public void Damage(float damage, Vector2 position, float knockbackForce)
    {
        if (isDead) return;
        damageHandler.ApplyDamage(damage, position, knockbackForce);

        if (life <= 0)
        {
            isDead = true;
            PlayRandomClip(hurtClips);
            spriteRenderer.enabled = false;
            collider.enabled = false;
            Destroy(gameObject, 1f); // Pequeno atraso para o som tocar
        }
        else
        {
            PlayRandomClip(hurtClips);
        }
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        var clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }
}
