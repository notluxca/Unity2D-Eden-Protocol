using UnityEngine;


// should make the cursor follow the mouse
//! defining gunController precision

public class CursorManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer CursorSprite;
    [SerializeField] private Sprite CantShootSprite;
    [SerializeField] private Sprite highPrecisionSprite; // precision 0.06
    [SerializeField] private Sprite mediumPrecisionSprite; // precision 0.20
    [SerializeField] private Sprite LowPrecisionSprite; // precision 0.25


    private GunController gunController;
    private Transform playerTransform;
    private float distanceFromPlayer;



    void Start()
    {
        Cursor.visible = false; // Desliga a vibilidade do mouse
        gunController = FindAnyObjectByType<GunController>();
        playerTransform = gunController.gameObject.transform;
        PlayerController.OnPlayerDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!playerTransform) return;
        mouseWorldPosition.z = playerTransform.position.z;
        transform.position = mouseWorldPosition;
        if (gunController.CanShoot)
        {
            EvaluatePrecisionAndSprite(mouseWorldPosition);
            // CursorSprite.sprite = highPrecisionSprite;
        }
        else
        {
            CursorSprite.sprite = CantShootSprite;
        }

    }

    private void EvaluatePrecisionAndSprite(Vector3 mousePosition)
    {
        distanceFromPlayer = Vector2.Distance(mousePosition, gunController.gameObject.transform.position);
        // Debug.Log(distanceFromPlayer);
        switch (distanceFromPlayer)
        {
            case <= 4f:
                CursorSprite.sprite = highPrecisionSprite;
                gunController.SetPrecision(0.06f);
                break;
            case <= 8f:
                CursorSprite.sprite = mediumPrecisionSprite;
                gunController.SetPrecision(0.15f);
                break;
            case > 10f:
                CursorSprite.sprite = LowPrecisionSprite;
                gunController.SetPrecision(0.30f);
                break;
        }

    }


}
