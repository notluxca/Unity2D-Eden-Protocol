using UnityEngine;

public class GunController : MonoBehaviour
{
    public float FireRate = 5f; // Tiros por segundo
    public float bulletSpeed = 10f;

    public Transform gunFirePoint;
    public GameObject bulletPrefab;
    public ArmPointer armPointer;

    private float fireCooldown = 0f;

    void Update()
    {
        // Atualiza o cooldown
        if (fireCooldown > 0)
            fireCooldown -= Time.deltaTime;

        // Verifica se pode atirar
        if (Input.GetMouseButton(0) && fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / FireRate;
        }
    }

    private void Shoot()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = gunFirePoint.position.z;

        Vector2 direction = (mouseWorldPosition - gunFirePoint.position).normalized;
        direction += new Vector2(0, Random.Range(-0.06f, 0.06f));
        direction.Normalize();

        GameObject bullet = ProjectilePool.Instance.GetProjectile(gunFirePoint.position, direction);
        // bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
}
