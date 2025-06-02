using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float scaleFactor = 1.1f;
    public float duration = 0.2f;
    public bool showMouse = false;
    public bool hideMouse = false;
    public GameObject reticula;
    public GunController gunController;

    void Start()
    {
        originalScale = transform.localScale;
        reticula = GameObject.FindWithTag("Reticula");
        gunController = FindFirstObjectByType<GunController>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (reticula != null) reticula.SetActive(false);
        if (gunController != null) gunController.ShootAlowed = false;
        if (showMouse) Cursor.visible = true;
        transform.DOScale(originalScale * scaleFactor, duration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (reticula != null) reticula.SetActive(true);
        if (gunController != null) gunController.ShootAlowed = true;
        if (hideMouse) Cursor.visible = false;
        transform.DOScale(originalScale, duration).SetEase(Ease.OutBack);
    }
}