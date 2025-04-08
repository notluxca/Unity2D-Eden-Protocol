using System.IO.Compression;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Horizontal movement speed
    public float thrust = 10f; // Jetpack thrust force
    public float flyImpulse = 10f; // Jetpack initial Thrust to get off ground
    public float tiltAmount = 15f; // Max tilt angle
    public float tiltSpeed = 5f; // Smooth tilt speed
    public Transform spriteTransform; // Sprite transform
    public SpriteRenderer spriteRenderer;
    public ParticleSystem playerParticleSystem; // Jetpack particle system


    private Rigidbody2D rb;
    private bool isThrustingLastFrame = false; // Track thrust state
    public float MaxSpeed = 10f;

    private bool Grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (Grounded)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) rb.AddForce(Vector2.up * flyImpulse, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        if (Grounded) GroundMovement();
        else JetpackMovement();
    }

    private void CorrectVelocity()
    {
        if (rb.linearVelocity.magnitude > MaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * MaxSpeed;
        }
    }

    private void GroundMovement()
    {
        spriteTransform.rotation = Quaternion.Lerp(spriteTransform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * tiltSpeed * 2);
        playerParticleSystem.Stop();


        float moveInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        CorrectSprite(moveInput);

        // Apply horizontal movement
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Correct speed if needed
        CorrectVelocity();
    }

    private void JetpackMovement()
    {
        float moveInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        bool isThrusting = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        CorrectSprite(moveInput);

        // Apply horizontal movement
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Apply vertical thrust
        if (isThrusting)
        {
            rb.AddForce(Vector2.up * thrust, ForceMode2D.Force);
            if (!isThrustingLastFrame) // Play only if not already playing
            {
                playerParticleSystem.Play();

            }
        }
        else
        {
            if (isThrustingLastFrame) // Stop only if previously thrusting
            {
                playerParticleSystem.Stop();

            }
        }

        isThrustingLastFrame = isThrusting; // Update thrust state

        // Correct speed if needed
        CorrectVelocity();

        // Smooth tilt effect based on X velocity
        float targetRotation = -rb.linearVelocity.x * tiltAmount / speed;
        spriteTransform.rotation = Quaternion.Lerp(spriteTransform.rotation, Quaternion.Euler(0, 0, targetRotation), Time.deltaTime * tiltSpeed);
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
}

