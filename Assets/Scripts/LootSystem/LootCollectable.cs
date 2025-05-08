using UnityEngine;

public class LootCollectable : MonoBehaviour
{
    public int value;
    public Rigidbody2D rb;
    private LootController lootController;

    private void OnEnable()
    {
        lootController = FindAnyObjectByType<LootController>();
        SpawnWithForce();
    }


    private void SpawnWithForce()
    {
        rb.rotation = Random.Range(0f, 360f);
        rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lootController.AddLoot(value);
            Destroy(gameObject);
        }
    }

}
