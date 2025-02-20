using System;
using System.Collections;
using System.Collections.Generic;
using SimpleCombat;
using SimpleCombat.Components;
using UnityEngine;
using UnityEngine.AI;
public class EnemyKnife : Enemy
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    
    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    
    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public Attack attack; // TESTING, IMPLEMENT BETTER LATER
    [SerializeField] public GameObject attackGameObject;
    
    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool allowStateChange;

    private void Awake() {
        // player = Player.Instance.gameObject.transform; \\ use in Start
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        attack = attackGameObject.GetComponent<Attack>();
        allowStateChange = true;
    }

    private void Update() {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if (!playerInSightRange && !playerInAttackRange && allowStateChange) Patrolling();
        if (playerInSightRange && !playerInAttackRange && allowStateChange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && allowStateChange) AttackPlayer();
        
        FlipSpriteToForwardVector(agent.desiredVelocity.normalized);
        isWalking = agent.velocity.sqrMagnitude > 0;
    }

    private void Patrolling() {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) {
            agent.SetDestination(walkPoint);
        }
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        
        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) {
            walkPointSet = false;
        }
    }

    private void SearchWalkPoint() {
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) {
            walkPointSet = true;
        }
    }

    private void ChasePlayer() {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer() {
        agent.SetDestination(transform.position);
        
        // transform.LookAt(player.position);

        if (!alreadyAttacked) {
            allowStateChange = false;

            // set Transform location of child gameObject attackGameObject to normalized vector of this gameObject to Player.Instance * 0.2
            // Calculate the direction from this GameObject to the Player.Instance
            Vector3 directionToPlayer = (Player.Instance.transform.position - transform.position).normalized;
            // Set the position of the child attackGameObject
            attackGameObject.transform.position = transform.position + directionToPlayer * 0.2f;

            combatController.Attack(attack);

            OnAttack();
            
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public override void OnAttackAnimationFinish() {
        allowStateChange = true;
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
