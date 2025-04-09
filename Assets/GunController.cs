using UnityEngine;

public class GunController : MonoBehaviour
{
    public float FireRate;
    public float bulletSpeed;

    public Transform gunFirePoint;
    public GameObject bulletPrefab; // make a pool of projectiles 
    public ArmPointer armPointer;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Shoot();
    }

    private void Shoot()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = gunFirePoint.position.z; // Keep the same z position
        Vector2 direction = (mouseWorldPosition - gunFirePoint.position).normalized;

        // Apply a slight random offset in bullet direction (2d game) so offset would be up or down
        direction += new Vector2(0, Random.Range(-0.06f, 0.06f));
        direction.Normalize();


        GameObject bullet = ProjectilePool.Instance.GetProjectile(gunFirePoint.position, direction);
        // bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
    }
}

