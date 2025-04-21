using UnityEngine;

public class DamageLayer : MonoBehaviour
{

    [SerializeField] private Sprite[] damageLayers; // Max size should be 8. The higher the number the less damaged sprite
    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        DomeController.OndomeHealthChange += OnDomeHealthChange;
        spriteRenderer.sprite = damageLayers[8];
    }

    private void Update()
    {

    }

    private void OnDestroy()
    {
        DomeController.OndomeHealthChange -= OnDomeHealthChange;
    }

    private void OnDomeHealthChange(int currentDomeHealth)
    {
        if (currentDomeHealth > damageLayers.Length || currentDomeHealth < 0) return;
        spriteRenderer.sprite = damageLayers[currentDomeHealth];
    }


}
