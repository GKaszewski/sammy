using System;
using System.Collections;
using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;
using VContainer;

public class LionAnimator : MonoBehaviour
{
    [SerializeField, Self] private Animator anim;
    [SerializeField, Self] private LionAI lionAI;
    [SerializeField, Self] private EnemyHealth lionHealth;
    [SerializeField] private float attackAnimTime = 0.5f;
    [SerializeField] private float deathAnimTime = 1.5f;

    private readonly int _idleHash = Animator.StringToHash("Idle");
    private readonly int _walkingHash = Animator.StringToHash("Walk");
    private readonly int _runningHash = Animator.StringToHash("Run");
    private readonly int _punchHash = Animator.StringToHash("Punch");
    private readonly int _deathHash = Animator.StringToHash("Death");

    private float _lockedTill;
    private int _currentState;
    private bool _attacked = false;
    private EventManager _eventManager;
    
    [Inject]
    private void Construct(EventManager eventManager)
    {
        _eventManager = eventManager;
    }

    private void Start()
    {
        _eventManager.OnLionAttack += OnAttack;
    }

    private void OnDisable()
    {
        _eventManager.OnLionAttack -= OnAttack;
    }

    private void OnAttack()
    {
        _attacked = true;
    }

    private void Update()
    {
        var state = GetState();
        _attacked = false;

        if (state == _currentState) return;
        anim.CrossFade(state, 0.1f, 0);
        _currentState = state;
    }

    private int GetState()
    {
        if (Time.time < _lockedTill) return _currentState;
        if (lionHealth.health <= 0) return LockState(_deathHash, deathAnimTime);
        if (_attacked) return LockState(_punchHash, attackAnimTime);
        if (lionAI.State is AIState.CHASING or AIState.FLEEING) return _runningHash;
        if (lionAI.State is AIState.PATROLING or AIState.WANDERING) return _walkingHash;
        return _idleHash;

        int LockState(int s, float t)
        {
            _lockedTill = Time.time + t;
            return s;
        }
    }
}