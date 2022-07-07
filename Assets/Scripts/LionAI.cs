using UnityEngine;
using UnityEngine.AI;

public enum AIState {
   IDLE,
   PATROLING,
   WANDERING,
   CHASING,
   ATTACKING
}

public class LionAI : MonoBehaviour {
   private NavMeshAgent agent;
   private float distanceFromPlayer;
   private float attackTimer = 0f;
   private int currentPatrolPoint = 0;

   public AIState state;
   public GameObject target;

   public float attackRange = 3.5f;
   public float rotationSpeed = 1f;
   public float attackRate = 1.75f;
   public float wanderRadius = 5f;

   public bool wanderMode = false;
   public bool patrolMode = false;

   public Transform[] patrolPoints;
   private void Start() {
      agent = GetComponent<NavMeshAgent>();
      agent.stoppingDistance = attackRange;
      GoToWanderPoint();
   }

   private void Update() {
      distanceFromPlayer = Vector3.Distance(transform.position, target.transform.position);
      if (distanceFromPlayer <= attackRange) {
         state = AIState.ATTACKING;
      }

      if (patrolPoints.Length == 0) patrolMode = false;
      if (patrolMode) wanderMode = false;
      if (wanderMode) patrolMode = false;

      if (state != AIState.ATTACKING) attackTimer = 0f;
      
      switch (state) {
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
      }
   }

   private Vector3 RandomNavSphere(Vector3 origin, float distance) {
      var randomDirection = UnityEngine.Random.insideUnitSphere * distance;
      randomDirection += origin;
      NavMeshHit navHit;
      NavMesh.SamplePosition(randomDirection, out navHit, distance, -1);
      return navHit.position;
   }

   private void Idle() {
      if (!patrolMode && !wanderMode) {
         agent.isStopped = true;
         agent.ResetPath();
      }

      if (wanderMode) state = AIState.WANDERING;

      if (patrolMode) state = AIState.PATROLING;
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
      attackTimer += Time.deltaTime;
      if (attackTimer > attackRate) DoAttack();
   }

   private void DoAttack() {
      var playerHealth = target.GetComponent<PlayerHealth>();
      if (playerHealth) playerHealth.TakeDamage();
      attackTimer = 0f;
   } 

   private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player")) {
         state = AIState.CHASING;
      }
   }

   private void OnTriggerStay(Collider other) {
      if (other.CompareTag("Player") && distanceFromPlayer > attackRange+0.5f) {
         state = AIState.CHASING;
      }
   }

   private void OnTriggerExit(Collider other) {
      if (other.CompareTag("Player")) {
         state = AIState.IDLE;
      }
   }
}
