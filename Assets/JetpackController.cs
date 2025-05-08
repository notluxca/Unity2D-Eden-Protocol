using UnityEngine;

public class JetpackController : MonoBehaviour
{
    public Rigidbody2D rb;
    public ParticleSystem particleSystem;
    public Transform spriteTransform;
    public PlayerController playerController;

    public float Thrust;
    private float tiltAmount;
    private float tiltSpeed;
    public float Speed;
    private float maxSpeed;

    private bool isThrustingLastFrame;

    private void Awake()
    {
        //! Results in null on rigidbody
        // playerController = GetComponent<PlayerController>();
        // rb = GetComponent<Rigidbody2D>();
        // particleSystem = GetComponentInChildren<ParticleSystem>();
        // spriteTransform = GetComponentInChildren<Transform>();

        Thrust = playerController.thrust;
        tiltAmount = playerController.tiltAmount;
        tiltSpeed = playerController.tiltSpeed;
        Speed = playerController.speed;
        maxSpeed = playerController.MaxSpeed;

    }

    public void HandleJetpackMovement(bool grounded)
    {
        float moveInput = Input.GetAxis("Horizontal");
        bool isThrusting = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        // Movimento horizontal
        rb.linearVelocity = new Vector2(moveInput * Speed, rb.linearVelocity.y);

        // Impulso vertical
        if (isThrusting)
        {
            rb.AddForce(Vector2.up * Thrust, ForceMode2D.Force);
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
            float targetRotation = -rb.linearVelocity.x * tiltAmount / Speed;
            spriteTransform.rotation = Quaternion.Lerp(spriteTransform.rotation, Quaternion.Euler(0, 0, targetRotation), Time.deltaTime * tiltSpeed);
        }
    }
}
