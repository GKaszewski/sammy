using UnityEngine;

public class Wasp : MonoBehaviour {
    public float livingTime = 5f;
    public int damage = 1;
    public float speed = 10f;

    protected void Start() {
        Destroy(gameObject, livingTime);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Lion")) {
            
            var obj = collision.collider.gameObject;
            obj.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
