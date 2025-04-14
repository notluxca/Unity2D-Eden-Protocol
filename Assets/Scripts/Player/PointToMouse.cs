using UnityEngine;

public class PointToMouse : MonoBehaviour
{



    void Update()
    {
        PointToMousePosition();
    }

    private void PointToMousePosition()
    {

        // should depend on player transform direction
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z; // Mantém a posição Z

        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}