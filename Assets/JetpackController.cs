using UnityEngine;

public class JetpackController : MonoBehaviour
{
    private Rigidbody2D rb;
    private ParticleSystem particleSystem;
    private Transform spriteTransform;

    private float thrust;
    private float tiltAmount;
    private float tiltSpeed;
    private float speed;
    private float maxSpeed;

    private bool isThrustingLastFrame;

    public JetpackController(Rigidbody2D rb, ParticleSystem particleSystem, Transform spriteTransform, float thrust, float tiltAmount, float tiltSpeed, float speed, float maxSpeed)
    {
        this.rb = rb;
        this.particleSystem = particleSystem;
        this.spriteTransform = spriteTransform;
        this.thrust = thrust;
        this.tiltAmount = tiltAmount;
        this.tiltSpeed = tiltSpeed;
        this.speed = speed;
        this.maxSpeed = maxSpeed;
    }

    public void HandleJetpackMovement(bool grounded)
    {
        float moveInput = Input.GetAxis("Horizontal");
        bool isThrusting = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        // Movimento horizontal
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

        // Impulso vertical
        if (isThrusting)
        {
            rb.AddForce(Vector2.up * thrust, ForceMode2D.Force);
            if (!isThrustingLastFrame)
                particleSystem.Play();
        }
        else
        {
            if (isThrustingLastFrame)
                particleSystem.Stop();
        }

        isThrustingLastFrame = isThrusting;

        // Corrige velocidade máxima
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // Inclinação baseada na velocidade
        if (!grounded)
        {
            float targetRotation = -rb.linearVelocity.x * tiltAmount / speed;
            spriteTransform.rotation = Quaternion.Lerp(spriteTransform.rotation, Quaternion.Euler(0, 0, targetRotation), Time.deltaTime * tiltSpeed);
        }
    }
}
