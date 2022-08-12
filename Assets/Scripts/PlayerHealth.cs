using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health {
    private PlayerCharacterController playerController;
    private Inventory inventory;
    public int lives = 5;
    public int maxLives = 5;
    public int maxHealth = 4;
    public Transform spawnpoint;

    public TMP_Text livesText;

    public float pushForce = 2f;
    
    private void Start() {
        inventory = GetComponent<Inventory>();
        playerController = GetComponent<PlayerCharacterController>();
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
        var heart = GameManager.Instance.playerUIManager.heartPrefab;
        var heartsList = GameManager.Instance.playerUIManager.heartsList;
        var newHeart = Instantiate(heart);
        newHeart.transform.SetParent(heartsList.transform);
        newHeart.GetComponent<RectTransform>().localScale = Vector3.one;
    }
    
    private void RemoveHeartFromUI() {
        var uiManager = GameManager.Instance.playerUIManager;
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
        playerController.Knockback(direction * pushForce);
    }

    public override void TakeDamage(int damage = 1){
        base.TakeDamage(damage);
        RemoveHeartFromUI();
        if (health <= 0) {
            ResetHealth();
            inventory.wasps = 0;
            inventory.DropCrystal();
            lives--;
            transform.position = spawnpoint.position;
        }
    }
}