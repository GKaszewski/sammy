using KBCore.Refs;
using Sammy;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class PlayerHealth : Health {
    [SerializeField, Self] private PlayerMover playerMover;
    [SerializeField, Self] private PlayerCrystalHandler crystalHandler;
    [SerializeField, Self] private Inventory inventory;
    public int lives = 5;
    public int maxLives = 5;
    public int maxHealth = 4;
    public Transform spawnpoint;

    public TMP_Text livesText;

    public float pushForce = 2f;
    
    private PlayerUIManager _playerUIManager;
    private EffectsManager _effectsManager;
    
    [Inject]
    private void Construct(PlayerUIManager playerUIManager, EffectsManager effectsManager) { 
        _playerUIManager = playerUIManager;
        _effectsManager = effectsManager;
    }
    
    private void Start() {
        ResetLives();
    }

    private void Update() {
        livesText.text = lives.ToString();
        if (lives <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            ResetLives();
        }
    }

    private void AddHeartToUI() {
        var heart = _playerUIManager.heartPrefab;
        var heartsList = _playerUIManager.heartsList;
        var newHeart = Instantiate(heart);
        newHeart.transform.SetParent(heartsList.transform);
        newHeart.GetComponent<RectTransform>().localScale = Vector3.one;
    }
    
    private void RemoveHeartFromUI() {
        var uiManager = _playerUIManager;
        var heartsList = uiManager.heartsList;
        if (heartsList.transform.childCount <= 0) return;
        var heart = heartsList.transform.GetChild(0);
        Destroy(heart.gameObject);
    }

    private void ResetLives() {
        lives = maxLives;
        ResetHealth();
    }

    private void ResetHealth() {
        health = maxHealth;
        for (var i = 0; i < health; i++) {
            AddHeartToUI();
        }
    }

    public void Push(Vector3 direction) {
        _ = playerMover.Knockback(direction * pushForce);
    }

    public override void TakeDamage(int damage){
        base.TakeDamage(damage);
        RemoveHeartFromUI();
        if (health <= 0) {
            ResetHealth();
            inventory.DecreaseWasps();
            crystalHandler.DropCrystal();
            lives--;
            _effectsManager.SpawnEffect(EffectType.DEATH, transform.position);
            transform.position = spawnpoint.position;
        }
    }
}