using System;
using System.Linq;
using UniRx;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public enum AIState {
   IDLE,
   PATROLING,
   WANDERING,
   CHASING,
   ATTACKING,
   FLEEING
}

public class LionAI : MonoBehaviour {
   private NavMeshAgent agent;
   private float distanceFromPlayer;
   private float attackTimer = 0f;
   private int currentPatrolPoint = 0;
   private Collider[] waspsDetectedColliders;
   private Transform wasp;
   private AIState previousStateBuffer;
   private AIState previousState;
   private AIState _state;
   private Vector3 fleePoint;
   private Transform startTransform;

   public AIState State {
      get => _state;
      set {
         previousStateBuffer = previousState;
         previousState = _state;
         if (previousState == value) previousState = previousStateBuffer;
         _state = value;
         GameManager.Instance.eventManager.LionAIStateChange(this, value);
      }
   }
   
   public GameObject target;
   public float attackRange = 3.5f;
   public float rotationSpeed = 1f;
   public float attackRate = 1.75f;
   public float wanderRadius = 5f;
   public float speed = 2f;
   public float acceleration = 5f;
   public float stoppingDistance = 0f;
   public float fleeingSpeed = 4f;
   public float fleeingAcceleration = 10f;
   public float waspDetectionRadius = 4f;

   public bool wanderMode = false;
   public bool patrolMode = false;

   public Transform[] patrolPoints;
   public LayerMask waspLayer;

   private void Start() {
      GameManager.Instance.eventManager.SpawnLion(this);
      agent = GetComponent<NavMeshAgent>();
      agent.stoppingDistance = attackRange;
      GoToWanderPoint();
   }

   private void OnDestroy() {
      GameManager.Instance.eventManager.DestroyLion(this);
   }

   private void Update() {
      distanceFromPlayer = Vector3.Distance(transform.position, target.transform.position);
      if (distanceFromPlayer <= attackRange) {
         State = AIState.ATTACKING;
      }

      if (patrolPoints.Length == 0) patrolMode = false;
      if (patrolMode) wanderMode = false;
      if (wanderMode) patrolMode = false;
      
      if (State != AIState.FLEEING) {
         ResetAgentProperties();
      }

      attackTimer += Time.deltaTime;
      
      DetectWasps();
      
      switch (State) {
         case AIState.IDLE:
            Idle();
            break;
         case AIState.PATROLING:
            Patrol();
            break;
         case AIState.WANDERING:
            Wander();
            break;
         case AIState.CHASING:
            Chase();
            break;
         case AIState.ATTACKING:
            Attack();
            break;
         case AIState.FLEEING:
            Flee();
            break;
      }
   }

   private void ResetAgentProperties() {
      agent.stoppingDistance = stoppingDistance;
      agent.speed = speed;
      agent.acceleration = acceleration;
      agent.autoBraking = false;
   }

   private void SetFleeingProperties() {
      agent.stoppingDistance = 0f;
      agent.speed = fleeingSpeed;
      agent.acceleration = fleeingAcceleration;
   }

   private Vector3 RandomNavSphere(Vector3 origin, float distance) {
      var randomDirection = UnityEngine.Random.insideUnitSphere * distance;
      randomDirection += origin;
      NavMeshHit navHit;
      NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);
      return navHit.position;
   }

   private void DetectWasps() {
      waspsDetectedColliders = Physics.OverlapSphere(transform.position, waspDetectionRadius, waspLayer);
      if ( waspsDetectedColliders.Length > 0) {
         wasp = waspsDetectedColliders
            .First(_wasp => _wasp.GetComponentInParent<NavigatingWasp>() != null).transform.parent;
         var waspScript = wasp.GetComponent<NavigatingWasp>();
         if ( waspScript.target == transform) State = AIState.FLEEING;
      }
      else {
         if (State == AIState.FLEEING) State = previousState;
      }
   }

   private void Idle() {
      if (!patrolMode && !wanderMode) {
         agent.isStopped = true;
         agent.ResetPath();
      }

      if (wanderMode) State = AIState.WANDERING;

      if (patrolMode) State = AIState.PATROLING;
   }

   private void Wander() {
      agent.autoBraking = true;
      if (agent.remainingDistance < attackRange || agent.isStopped) {
         GoToWanderPoint();
      }
   }

   private void GoToWanderPoint() {
      var newDestination = RandomNavSphere(transform.position, wanderRadius);
      agent.SetDestination(newDestination);
   }
   
   private void Patrol() {
      agent.autoBraking = false;
      if (!agent.pathPending && agent.remainingDistance < attackRange) {
         GoToNextPoint();
      }
   }

   private void Flee() {
      startTransform = transform;
      var direction = (wasp.position - transform.position).normalized;
      transform.rotation = quaternion.LookRotation(-direction, Vector3.up);
      fleePoint = transform.position + transform.forward * 5f;
      NavMeshHit hit;
      NavMesh.SamplePosition(fleePoint, out hit, 5f, 1 << NavMesh.GetAreaFromName("Walkable"));
      transform.position = startTransform.position;
      transform.rotation = startTransform.rotation;
      SetFleeingProperties();
      agent.SetDestination(hit.position);
   }

   private void GoToNextPoint() {
      if (patrolPoints.Length == 0) return;
      agent.SetDestination(patrolPoints[currentPatrolPoint].position);
      currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
   }

   private void Chase() {
      agent.ResetPath();
      agent.isStopped = false;
      var direction = (target.transform.position - transform.position).normalized;
      var singleStep = rotationSpeed * Time.deltaTime;
      var newDirection = Vector3.RotateTowards(transform.forward, direction, singleStep, 0f);
      transform.rotation = Quaternion.LookRotation(newDirection);
      agent.SetDestination(target.transform.position);
   }

   private void Attack() {
      if (attackTimer > attackRate) DoAttack();
   }

   private void DoAttack() {
      GameManager.Instance.eventManager.LionAttack();
      var playerHealth = target.GetComponent<PlayerHealth>();
      if (playerHealth) playerHealth.TakeDamage();
      attackTimer = 0f;
   } 

   private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player")) {
         State = AIState.CHASING;
      }
   }

   private void OnTriggerStay(Collider other) {
      if (other.CompareTag("Player") && distanceFromPlayer > attackRange+0.5f) {
         State = AIState.CHASING;
      }
   }

   private void OnTriggerExit(Collider other) {
      if (other.CompareTag("Player")) {
         State = AIState.IDLE;
      }
   }

   private void OnDrawGizmos() {
      Gizmos.DrawSphere(fleePoint, 1f);
   }
}
