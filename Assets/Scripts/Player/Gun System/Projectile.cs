using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private ParticleSystem particleSystem;
    public float ProjectileDamage = 1;
    public float speed = 2f;
    public bool active = false;
    private Vector3 currentDir;
    private TrailRenderer trailRenderer;
    public LayerMask layerMask = 8;
    Collider2D collider2D;
    SpriteRenderer spriteRenderer;
    AudioSource audioSource;
    public AudioClip hitClip;

    public float knockbackForce;

    private Coroutine lifeCoroutine; // <- referência da corrotina para poder parar depois

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        collider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Init(Vector3 direction)
    {
        currentDir = direction;
        active = true;

        particleSystem.Clear();
        trailRenderer.Clear();
        collider2D.enabled = true;
        spriteRenderer.enabled = true;

        if (lifeCoroutine != null)
            StopCoroutine(lifeCoroutine);

        lifeCoroutine = StartCoroutine(LifetimeCoroutine());
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        if (active)
        {
            ProjectilePool.Instance.ReturnToPool(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (active)
        {
            MoveToDirection();
        }
    }

    private void MoveToDirection()
    {
        transform.Translate(currentDir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!active) return;

        if ((layerMask.value & (1 << other.gameObject.layer)) == 0)
            return;

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.Damage(ProjectileDamage, transform.position, knockbackForce);
        }


        active = false;
        audioSource.PlayOneShot(hitClip);
        collider2D.enabled = false;
        spriteRenderer.enabled = false;
        particleSystem.Play();

        if (lifeCoroutine != null)
            StopCoroutine(lifeCoroutine); // <- interrompe o timer de vida se colidiu

        StartCoroutine(ReturnAfterParticle());
    }

    private IEnumerator ReturnAfterParticle()
    {
        yield return new WaitForSeconds(0.3f);
        ProjectilePool.Instance.ReturnToPool(gameObject);
    }
}
