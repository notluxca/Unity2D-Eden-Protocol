using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed = 2f;
    public bool active = false;
    private Vector3 currentDir;
    public LayerMask layerMask = 8;


    public void Init(Vector3 direction)
    {
        currentDir = direction;
        active = true;
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
        Debug.Log("other: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hit");
            Destroy(other.gameObject);
            ProjectilePool.Instance.ReturnToPool(this.gameObject);
        }
    }





}
