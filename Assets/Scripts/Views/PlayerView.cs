using Sammy.Interfaces;
using UnityEngine;

namespace Sammy.Views
{
    public class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform waspSpawner;
        [SerializeField] private GameObject basicWaspPrefab;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private GameObject crystalPrefab;

        public Transform Transform => transform;
        public bool IsGrounded => characterController.isGrounded;

        public void Move(Vector3 velocity)
        {
            characterController.Move((velocity + Physics.gravity) * Time.deltaTime);
        }

        public void Rotate(float direction)
        {
            transform.Rotate(Vector3.up * (direction * rotationSpeed * Time.deltaTime));
        }

        public void ApplyJumpForce()
        {
            var jumpVelocity = new Vector3(0, jumpForce, 0);
            characterController.Move(jumpVelocity * Time.deltaTime);
        }

        public void SpawnWasp(WaspType type)
        {
            Instantiate(basicWaspPrefab, waspSpawner.position, waspSpawner.rotation);
        }

        public void SpawnDroppedCrystal(CrystalColor color)
        {
            var spawnPos = transform.position + (transform.forward * 2f);
            var newCrystalGo = Instantiate(crystalPrefab, spawnPos, Quaternion.identity);
            var newCrystal = newCrystalGo.GetComponent<Crystal>();

            if (!newCrystal) return;
            
            newCrystal.color = color;
            newCrystal.HandleMaterial();
            newCrystal.player = transform;
        }
    }
}