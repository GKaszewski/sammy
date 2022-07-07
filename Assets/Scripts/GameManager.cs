using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public EventManager eventManager;
    public PlayerUIManager playerUIManager;

    public int maxPoints;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
        
        eventManager = new EventManager();
    }

    private void Start() {
        AudioManager.instance.Play("Music");
    }
}