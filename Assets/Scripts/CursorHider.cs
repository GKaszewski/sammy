using TouchControlsKit;
using UnityEngine;

public class CursorHider : MonoBehaviour {
    private bool _cursorLocked;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _cursorLocked = true;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !_cursorLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cursorLocked = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _cursorLocked) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _cursorLocked = false;
        }
    }
}