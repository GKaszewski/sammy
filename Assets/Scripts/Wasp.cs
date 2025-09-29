using UnityEngine;
using VContainer;

public class Wasp : MonoBehaviour {
    public float speed = 10f;
    
    [SerializeField] private float livingTime = 5f;
    [SerializeField] private int damage = 1;

    private EffectsManager _effectsManager;

    [Inject]
    private void Construct(EffectsManager effectsManager)
    {
        _effectsManager = effectsManager;
    }

    protected void Start() {
        AudioManager.Instance.Play("fly flying");
        Destroy(gameObject, livingTime);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Lion")) {
            
            var obj = collision.collider.gameObject;
            obj.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            _effectsManager.SpawnEffect(EffectType.HIT, transform.position);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
