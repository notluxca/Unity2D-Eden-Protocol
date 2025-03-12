using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Horizontal movement speed
    public float thrust = 10f; // Jetpack thrust force
    public float tiltAmount = 15f; // Max tilt angle
    public float tiltSpeed = 5f; // Smooth tilt speed
    public Transform spriteTransform; // Sprite transform
    public ParticleSystem playerParticleSystem; // Jetpack particle system

    private Rigidbody2D rb;
    private bool isThrustingLastFrame = false; // Track thrust state

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input
        float moveInput = Input.GetAxis("Horizontal"); // A/D or Left/Right Arrow
        bool isThrusting = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

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

        // Smooth tilt effect based on X velocity
        float targetRotation = -rb.linearVelocity.x * tiltAmount / speed; 
        spriteTransform.rotation = Quaternion.Lerp(spriteTransform.rotation, Quaternion.Euler(0, 0, targetRotation), Time.deltaTime * tiltSpeed);
    }
}
