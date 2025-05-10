using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamageable
{
    public static event Action OnPlayerDeath;

    public float life = 6f;
    public float speed = 5f;
    public float thrust = 10f;
    public float flyImpulse = 10f;
    public float tiltAmount = 15f;
    public float tiltSpeed = 5f;
    public float MaxSpeed = 10f;

    public float damageCooldown = 1f; // cooldown entre danos
    private bool canTakeDamage = true;

    public Transform spriteTransform;
    public Transform gunFirePoint;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem playerParticleSystem;

    public AudioClip damageClip;
    public bool Grounded;

    private Rigidbody2D rb;
    private JetpackController jetpackController;
    [SerializeField] private AudioSource audioSource;
    private Color originalColor;

    public object knockbackTimer { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        jetpackController = GetComponent<JetpackController>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        CorrectSprite(moveInput);

        if (Grounded && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        {
            rb.linearVelocity += Vector2.up * flyImpulse;
        }
    }

    void FixedUpdate()
    {
        if (Grounded) GroundMovement();
        else jetpackController.HandleJetpackMovement(Grounded);
    }

    private void GroundMovement()
    {
        playerParticleSystem.Stop();

        float moveInput = Input.GetAxis("Horizontal");
        CorrectSprite(moveInput);

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        if (rb.linearVelocity.magnitude > MaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * MaxSpeed;
        }
    }

    private void CorrectSprite(float inputDir)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x > transform.position.x)
        {
            spriteTransform.localScale = new Vector3(-Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
        else
        {
            spriteTransform.localScale = new Vector3(Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Grounded = false;
        }
    }

    public void Damage(float damage, Vector2 position, float knockbackForce)
    {
        if (!canTakeDamage) return;
        StartCoroutine(DamageCoroutine(damage, position, knockbackForce));
    }

    private IEnumerator DamageCoroutine(float damage, Vector2 position, float knockbackForce)
    {
        canTakeDamage = false;

        life -= damage;

        if (audioSource && damageClip)
            audioSource.PlayOneShot(damageClip);

        StartCoroutine(FlashRed());

        Vector2 knockbackDirection = (Vector2)transform.position - position;
        knockbackDirection.Normalize();

        if (rb != null)
        {
            Debug.Log("Knockback applied");
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (life <= 0)
        {
            OnPlayerDeath?.Invoke();
            Destroy(gameObject);
        }

        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = originalColor;
    }
}
