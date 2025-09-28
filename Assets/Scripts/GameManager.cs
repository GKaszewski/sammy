using System;
using TouchControlsKit;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public EventManager eventManager;
    public PlayerUIManager playerUIManager;
    public AIManager aiManager;
    public EffectsManager effectsManager;

    public int maxPoints;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
        
        eventManager = new EventManager();
    }
}