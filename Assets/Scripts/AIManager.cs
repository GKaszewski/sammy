using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

public class AIManager : MonoBehaviour {
    private EventManager _eventManager;
    
    public List<LionAI> lions = new();
    public GameObject chaseLight;
    
    [Inject]
    private void Construct(EventManager eventManager) {
        _eventManager = eventManager;
    }

    private void Start() {
        chaseLight = GameObject.FindWithTag("ChaseLight");
        _eventManager.OnLionSpawn += OnLionSpawn;
        _eventManager.OnLionDestroy += OnLionDestroy;
        _eventManager.OnLionAIStateChange += OnAIStateChange;
    }

    private void OnLionDestroy(LionAI obj) {
        if (!lions.Contains(obj)) return;
        lions.Remove(obj);
        
        if(lions.Count == 0) chaseLight.SetActive(false);
    }

    private void OnLionSpawn(LionAI obj) {
        if (lions.Contains(obj)) return;
        lions.Add(obj);
    }

    private void OnDisable() {
        _eventManager.OnLionAIStateChange -= OnAIStateChange;
        _eventManager.OnLionSpawn -= OnLionSpawn;
        _eventManager.OnLionDestroy -= OnLionDestroy;
    }

    private void CheckIfPlayerIsChased() {
        var atLeastOneLionIsChasing = lions.Count(lion => lion.State == AIState.CHASING) > 0;
        chaseLight.SetActive(atLeastOneLionIsChasing);
    }

    private void OnAIStateChange(LionAI ai, AIState state) {
        CheckIfPlayerIsChased();
    }
}