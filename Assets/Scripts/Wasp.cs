using UnityEngine;

public class Wasp : MonoBehaviour {
    public float livingTime = 5f;

    private void Start() {
        Destroy(gameObject, livingTime);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Lion")) {
            Debug.Log("Hit enemy!");
            var obj = collision.collider.gameObject;
            if (obj) Destroy(obj);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
