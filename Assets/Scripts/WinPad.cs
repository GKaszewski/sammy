using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class WinPad : MonoBehaviour {
    [SerializeField] private float waitTime = 2.5f;
    
    private EventManager _eventManager;

    [Inject]
    private void Construct(EventManager eventManager)
    {
        _eventManager = eventManager;
    }
    
    private async void OnControllerColliderHit(ControllerColliderHit hit)
    {
        try
        {
            if (hit.collider.CompareTag("WinPad")) {
                _eventManager.Win();
                await Task.Delay(TimeSpan.FromSeconds(waitTime));
                var nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
                SceneManager.LoadScene(nextIndex);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in WinPad OnControllerColliderHit: {e.Message}");
        }
    }
}