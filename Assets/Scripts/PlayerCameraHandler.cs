using UnityEngine;
using VContainer;

namespace Sammy
{
    public class PlayerCameraHandler : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;

        [Header("Settings")]
        [SerializeField] private float quickRotationTime = 0.2f;
        
        private PlayerInput _playerInput;

        [Inject]
        private void Construct(PlayerInput playerInput)
        {
            _playerInput = playerInput;
        }
        
        private void Update()
        {
            if (_playerInput.QuickTurnInput != 0f)
            {
                HandleQuickTurn(_playerInput.QuickTurnInput);
            }
        }
        
        private void HandleQuickTurn(float direction)
        {
            AudioManager.Instance.Play("quickturn");

            var angle = 90f * direction;

            LeanTween.rotateAroundLocal(gameObject, Vector3.up, angle, quickRotationTime);
            LeanTween.rotateAroundLocal(playerTransform.gameObject, Vector3.up, angle, quickRotationTime);
        }
    }
}