using System.ComponentModel;
using UnityEngine;

public class ArmPointer : MonoBehaviour
{
    public Transform ArmAnchorPoint;
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
        mousePosition.z = transform.position.z; // Mantém a posição Z

        Vector3 direction = mousePosition - ArmAnchorPoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (spriteOrientation == 1) angle += 180;
        ArmAnchorPoint.rotation = Quaternion.Euler(0f, 0f, angle);
    }


    // identifica a direção a qual o sprite está virado (1 = direita, -1 = esquerda)
    private void UpdateSpriteOrientation()
    {
        if (spriteTransform.localScale.x > 0) spriteOrientation = -1;
        else spriteOrientation = 1;
    }


}
