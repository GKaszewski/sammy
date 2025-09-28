using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] private List<GameObject> screens = new();

    public void LoadLevel(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ActivateScreen(GameObject screenToActivate) {
        foreach (var screen in screens)
        {
            screen.SetActive(screen == screenToActivate);
        }
    }
}