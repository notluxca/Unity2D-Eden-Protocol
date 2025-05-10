using UnityEngine;

public class GunController : MonoBehaviour
{
    public float FireRate = 5f; // Tiros por segundo
    public float bulletSpeed = 10f;

    public Transform gunFirePoint;
    public GameObject bulletPrefab;
    public ArmPointer armPointer;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] shootClips; // <- seus 3 sons de tiro
    public float soundCooldown = 0.05f;

    private float fireCooldown = 0f;
    private float lastShootSoundTime = -999f;
    private float precisionOffset = 0.06f;

    public bool CanShoot => fireCooldown <= 0f;

    void Update()
    {
        // Atualiza o cooldown de tiro
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
        direction += new Vector2(0, Random.Range(-precisionOffset, precisionOffset));
        direction.Normalize();

        GameObject bullet = ProjectilePool.Instance.GetProjectile(gunFirePoint.position, direction);

        // Toca som com cooldown
        if (Time.time - lastShootSoundTime > soundCooldown && shootClips.Length > 0)
        {
            AudioClip randomClip = shootClips[Random.Range(0, shootClips.Length)];
            audioSource.PlayOneShot(randomClip);
            lastShootSoundTime = Time.time;
        }
    }

    public void SetPrecision(float value)
    {
        precisionOffset = value;
    }
}
