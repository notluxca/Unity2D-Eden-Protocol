using System.ComponentModel;
using UnityEngine;

public class ArmPointer : MonoBehaviour
{

    public Transform ArmAnchorPoint;
    public Vector2 CurrentArmDirection => AngleToDirection(currentArmAngle);
    public float currentArmAngle;

    public Transform spriteTransform;
    Vector3 mousePosition;
    [SerializeField, ReadOnly(true)] private int spriteOrientation = -1; // considerando que o sprite virado começa para esquerda



    private void Update()
    {
        PointToMousePosition();
    }


    private void PointToMousePosition()
    {
        UpdateSpriteOrientation();
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;

        Vector3 direction = mousePosition - ArmAnchorPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (spriteOrientation == 1) angle += 180;

        ArmAnchorPoint.rotation = Quaternion.Euler(0f, 0f, angle);

        // Atualiza o ângulo
        currentArmAngle = angle;
    }

    // identifica a direção a qual o sprite está virado (1 = direita, -1 = esquerda)
    private void UpdateSpriteOrientation()
    {
        if (spriteTransform.localScale.x > 0) spriteOrientation = -1;
        else spriteOrientation = 1;
    }

    private Vector2 AngleToDirection(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }


}
