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
        Instance = this;

        for (int i = 0; i < initialPoolSize; i++)
        {
            var obj = Instantiate(projectilePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetProjectile(Vector2 position, Vector2 direction)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(projectilePrefab);
        obj.transform.position = position;
        obj.SetActive(true);

        obj.GetComponent<Projectile>().Init(direction);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
