using UnityEngine;

public class Wasp : MonoBehaviour {
    public float livingTime = 5f;
    public int damage = 1;
    public float speed = 10f;

    protected void Start() {
        AudioManager.instance.Play("fly flying");
        Destroy(gameObject, livingTime);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Lion")) {
            
            var obj = collision.collider.gameObject;
            obj.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            GameManager.Instance.effectsManager.SpawnEffect(EffectType.HIT, transform.position);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
