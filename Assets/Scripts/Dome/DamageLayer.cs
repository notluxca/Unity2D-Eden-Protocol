using UnityEngine;

public class DamageLayer : MonoBehaviour
{
    [SerializeField] private Sprite[] damageLayers; // do menos danificado (índice 0) ao mais danificado (último)

    public SpriteRenderer spriteRenderer;
    public DomeController domeController;

    private void Start()
    {
        // spriteRenderer = GetComponent<SpriteRenderer>();
        DomeController.OndomeHealthChange += OnDomeHealthChange;
        UpdateDomeSprite((int)domeController.initialLife);
    }

    // #if UNITY_EDITOR
    //     private void OnValidate()
    //     {
    //         if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    //         if (spriteRenderer != null) spriteRenderer.sprite = null;
    //     }
    // #endif

    private void OnDestroy()
    {
        DomeController.OndomeHealthChange -= OnDomeHealthChange;
    }

    private void OnDomeHealthChange(int currentDomeHealth)
    {
        UpdateDomeSprite(currentDomeHealth);
    }

    private void UpdateDomeSprite(int currentHealth)
    {
        // Debug.Log("currentHealth: " + currentHealth);
        if (damageLayers == null || damageLayers.Length == 0) return;
        if (domeController.currentLife == 1)
        {
            spriteRenderer.sprite = damageLayers[9];
            return;
        }
        else if (domeController.currentLife == 0)
        {
            spriteRenderer.sprite = damageLayers[10];
            return;
        }
        else
        {
            // Calcula o índice do sprite com base na proporção da vida
            float healthRatio = Mathf.Clamp01((float)currentHealth / (int)domeController.initialLife);
            int spriteIndex = Mathf.RoundToInt((1f - healthRatio) * (damageLayers.Length - 1));
            spriteRenderer.sprite = damageLayers[spriteIndex];
        }
    }
}
