using System;
using UnityEngine;

public class PlayerHealth : Health{
    public int health = 4;

    private void Start() {
        for (var i = 0; i < health; i++) {
            AddHeartToUI();
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

    public override void TakeDamage(int damage = 1){
        health -= damage;
        Debug.Log("took damage");
        RemoveHeartFromUI();
        if(health <= 0){
            //GameManager.Save();
            //SceneManager.LoadSceneAsync(2);
            Debug.Log("Game Over");
            gameObject.SetActive(false);
        }
    }
}