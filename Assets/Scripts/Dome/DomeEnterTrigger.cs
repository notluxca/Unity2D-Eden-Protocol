using UnityEngine;

public class DomeEnterTrigger : MonoBehaviour
{

    public DomeController domeController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            domeController.EnterDome();
        }
    }
}
