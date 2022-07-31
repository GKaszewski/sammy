using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public List<GameObject> screens = new();

    public void LoadLevel(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ActivateScreen(GameObject screenToActivate) {
        foreach (var screen in screens) {
            if (screen == screenToActivate) screen.SetActive(true);
            else screen.SetActive(false);
        }
    }
}