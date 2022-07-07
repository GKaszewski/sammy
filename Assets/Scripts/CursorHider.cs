using UnityEngine;

public class CursorHider : MonoBehaviour {
    private bool cursorLocked;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !cursorLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorLocked = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && cursorLocked) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorLocked = false;
        }
    }
}