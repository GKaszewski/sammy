using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Point : MonoBehaviour {
    public int points;
    public float bounceLength = 3f;
    public float bounceTime = 2f;
    public float variation = 0.2f;

    private void Start() {
        var random = Random.Range(-variation, variation);
        var sign = Mathf.Sign(random);
        var moveTo = transform.position.y + (sign * bounceLength) + random;
        LeanTween.cancel(gameObject);
        LeanTween.moveLocalY(gameObject, moveTo , bounceTime).setEaseInOutSine().setLoopPingPong();
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        
        var inventory = other.GetComponent<Inventory>();
        if (!inventory) return;
        
        AudioManager.instance.Play("points pickup");
        inventory.CollectPoints(points);
        Die();
    }

    private void Die() {
        Destroy(gameObject);
    }
}