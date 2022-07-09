using System;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrystalSpawner : MonoBehaviour {
        private Vector3 spawnDirection = Vector3.up;

        public Crystal crystalPrefab;
        public CrystalColor color;
        public bool randomColor = false;
        public Transform spawnPosition;

        public float spawnForce = 10f;
        public float spawnRate = 1.5f;
        public float cleanupTime = 5f;

        public bool isActive = true;

        private void Start() {
                InvokeRepeating(nameof(Spawn), 0f, spawnRate);
        }

        private async void Spawn() {
                if (!isActive) return;
                if (randomColor) color = (CrystalColor) Random.Range(0, 6);
                var newCrystal = Instantiate(crystalPrefab, spawnPosition.position, Quaternion.identity)
                        .GetComponent<Crystal>();
                newCrystal.color = color;
                var rb = newCrystal.GetComponent<Rigidbody>();
                if (rb) rb.AddForce(spawnDirection * spawnForce, ForceMode.Impulse);
                await Task.Delay(TimeSpan.FromSeconds(cleanupTime));
                if(newCrystal) newCrystal.Die();
        }
}