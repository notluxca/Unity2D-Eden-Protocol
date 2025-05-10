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
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            audioSource.PlayOneShot(attackClip);
            damageable?.Damage(AttackDamage, transform.position, 5);
        }
    }

    public void Damage(float damage, Vector2 position, float knockbackForce)
    {
        if (isDead) return;

        life -= damage;

        if (life <= 0)
        {
            isDead = true;
            // PlayRandomClip(damageClips);
            // Aqui você pode adicionar lógica de morte, animação, loot drop etc.
            Destroy(gameObject, 0.5f); // Pequeno atraso para o som tocar
        }
        else
        {
            PlayRandomClip(hurtClips);
        }

        damageHandler.ApplyDamage(damage, position, knockbackForce);
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        var clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }
}
