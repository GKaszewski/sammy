using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionAnimator : MonoBehaviour {
        private Animator anim;
       
       private readonly int IdleHash = Animator.StringToHash("Idle");
       private readonly int WalkingHash = Animator.StringToHash("Walk");
       private readonly int RunningHash = Animator.StringToHash("Run");
       private readonly int PunchHash = Animator.StringToHash("Punch");
       private readonly int DeathHash = Animator.StringToHash("Death");

       private float lockedTill;
       private int currentState;

       private bool attacked = false;
       
       private LionAI lionAI;
       private EnemyHealth lionHealth;

       public float attackAnimTime = 0.5f;
       public float deathAnimTime = 1.5f;
   
       private void Start() {
           anim = GetComponent<Animator>();
           lionAI = GetComponent<LionAI>();
           lionHealth = GetComponent<EnemyHealth>();
           GameManager.Instance.eventManager.OnLionAttack += OnAttack;
       }

       private void OnDisable() {
           GameManager.Instance.eventManager.OnLionAttack -= OnAttack;
       }

       private void OnAttack() {
           attacked = true;
       }

       private void Update() {
           var state = GetState();
           attacked = false;
           
           if (state == currentState) return;
           anim.CrossFade(state, 0.1f, 0);
           currentState = state;
       }
   
       private int GetState() {
           if (Time.time < lockedTill) return currentState;
           if (lionHealth.health <= 0) return LockState(DeathHash, deathAnimTime);
           if (attacked) return LockState(PunchHash, attackAnimTime);
           if (lionAI.State is AIState.CHASING or AIState.FLEEING) return RunningHash;
           if (lionAI.State is AIState.PATROLING or AIState.WANDERING) return WalkingHash;
           return IdleHash;
           
           int LockState(int s, float t) {
               lockedTill = Time.time + t;
               return s;
           }
       }
}
