using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour
{
    public Texture2D m_cursor;

    public void OnPointerEnter()
    {
        Debug.Log("onpointerenter");
        Cursor.SetCursor(m_cursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
