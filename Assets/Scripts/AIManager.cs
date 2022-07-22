using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIManager : MonoBehaviour {
    public List<LionAI> lions = new();
    public GameObject chaseLight;

    private void Start() {
        chaseLight = GameObject.FindWithTag("ChaseLight");
        GameManager.Instance.eventManager.OnLionSpawn += OnLionSpawn;
        GameManager.Instance.eventManager.OnLionDestroy += OnLionDestroy;
        GameManager.Instance.eventManager.OnLionAIStateChange += OnAIStateChange;
    }

    private void OnLionDestroy(LionAI obj) {
        if (!lions.Contains(obj)) return;
        lions.Remove(obj);
    }

    private void OnLionSpawn(LionAI obj) {
        if (lions.Contains(obj)) return;
        lions.Add(obj);
    }

    private void OnDisable() {
        GameManager.Instance.eventManager.OnLionAIStateChange -= OnAIStateChange;
        GameManager.Instance.eventManager.OnLionSpawn -= OnLionSpawn;
        GameManager.Instance.eventManager.OnLionDestroy -= OnLionDestroy;
    }

    private void CheckIfPlayerIsChased() {
        var atLeastOneLionIsChasing = lions.Count(lion => lion.State == AIState.CHASING) > 0;
        chaseLight.SetActive(atLeastOneLionIsChasing);
    }

    private void OnAIStateChange(LionAI ai, AIState state) {
        CheckIfPlayerIsChased();
    }
}