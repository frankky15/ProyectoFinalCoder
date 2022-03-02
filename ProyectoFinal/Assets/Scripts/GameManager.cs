using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool cursorIsLocked = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            cursorIsLocked = !cursorIsLocked;

        if (cursorIsLocked)
        {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        }

        if (!cursorIsLocked)
        {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        }
    }
}
