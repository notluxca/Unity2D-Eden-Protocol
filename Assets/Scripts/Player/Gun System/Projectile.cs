using UnityEngine;

public class Projectile : MonoBehaviour
{

    private ParticleSystem particleSystem;
    public float ProjectileDamage = 1; // 1 default
    public float speed = 2f;
    public bool active = false;
    private Vector3 currentDir;
    private TrailRenderer trailRenderer;
    public LayerMask layerMask = 8;


    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }


    public void Init(Vector3 direction)
    {
        currentDir = direction;
        active = true;
        particleSystem.Clear();
        trailRenderer.Clear();
        Invoke("DestroySelf", 5f);
    }

    void DestroySelf()
    {
        if (active)
        {
            ProjectilePool.Instance.ReturnToPool(this.gameObject);
        }
    }


    void FixedUpdate()
    {

        if (active)
        {
            MoveToDirection();
            // Debug.Log("blablabla");
        }
    }

    private void MoveToDirection()
    {
        // Debug.Log(currentDir);
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
            damageable.Damage(ProjectileDamage);
        }

        particleSystem.Play();
        ProjectilePool.Instance.ReturnToPool(this.gameObject);
    }





}
