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
        Debug.Log(currentDir);
        transform.Translate(currentDir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == layerMask)
        {
            Destroy(other.gameObject); // provisorio
            Destroy(gameObject); // provisorio

        }
    }




}
