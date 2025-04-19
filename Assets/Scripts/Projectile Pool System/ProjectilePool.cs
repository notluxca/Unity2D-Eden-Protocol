using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int initialPoolSize = 20;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewProjectile();
        }
    }

    private GameObject CreateNewProjectile()
    {
        GameObject obj = Instantiate(projectilePrefab);
        obj.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public GameObject GetProjectile(Vector2 position, Vector2 direction)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : CreateNewProjectile();
        obj.transform.position = position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        obj.GetComponent<Projectile>().Init(direction);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
