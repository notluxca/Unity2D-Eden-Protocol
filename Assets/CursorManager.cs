using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    void Start()
    {
        cursorTexture.filterMode = FilterMode.Point;
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
