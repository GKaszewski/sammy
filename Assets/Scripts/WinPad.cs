using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPad : MonoBehaviour {
    public float waitTime = 2.5f;
    private async void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.collider.CompareTag("WinPad")) {
            GameManager.Instance.eventManager.Win();
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
            var nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextIndex);
        }
    }
}