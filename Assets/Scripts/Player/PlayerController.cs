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

    public float damageCooldown = 1f;
    private bool canTakeDamage = true;
    private bool isDead = false;

    public Transform spriteTransform;
    public Transform gunFirePoint;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem playerParticleSystem;

    public AudioClip damageClip;
    public AudioClip hitGround;
    public AudioClip hitDome;
    public AudioClip PlayerDeathSound;
    public bool Grounded = false;

    private Rigidbody2D rb;
    private JetpackController jetpackController;
    private GunController gunController;
    private ArmPointer pointToMouse;
    [SerializeField] private AudioSource audioSource;
    private Color originalColor;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        jetpackController = GetComponent<JetpackController>();
        gunController = GetComponentInChildren<GunController>();
        pointToMouse = GetComponent<ArmPointer>();
        originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (isDead) return;

        float moveInput = Input.GetAxis("Horizontal");
        CorrectSprite(moveInput);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        jetpackController.HandleJetpackMovement(Grounded);
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
        switch (other.gameObject.tag)
        {
            case "Ground":
                audioSource.PlayOneShot(hitGround);
                break;
            case "Dome":
                audioSource.PlayOneShot(hitDome);
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // Grounded = false;
        }
    }

    public void Damage(float damage, Vector2 position, float knockbackForce)
    {
        if (!canTakeDamage || isDead) return;
        StartCoroutine(DamageCoroutine(damage, position, knockbackForce));
    }

    private IEnumerator DamageCoroutine(float damage, Vector2 position, float knockbackForce)
    {
        canTakeDamage = false;

        life -= damage;

        if (audioSource && damageClip) // fast fix to deathsound not playing
            if (life <= 0)
            {
                audioSource.PlayOneShot(PlayerDeathSound);
            }
            else
            {
                audioSource.PlayOneShot(damageClip);
            }


        StartCoroutine(FlashRed());

        Vector2 knockbackDirection = (Vector2)transform.position - position;
        knockbackDirection.Normalize();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (life <= 0)
        {
            Die();
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

    private void Die()
    {
        isDead = true;
        OnPlayerDeath?.Invoke();

        // Desativar GunController e Jetpack
        if (gunController != null)
            gunController.enabled = false;

        if (jetpackController != null)
            jetpackController.enabled = false;

        if (pointToMouse != null)
            pointToMouse.enabled = false;

        // Parar partículas
        if (playerParticleSystem != null)
            playerParticleSystem.Stop();

        // Liberar rotação e aplicar torque para cair
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddTorque(0.5f, ForceMode2D.Impulse);
        }

        // Rotaciona o corpo para cair de lado
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }
}
