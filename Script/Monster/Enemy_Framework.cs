using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // For NavMeshAgent

public class Enemy_Framework : MonoBehaviour
{
    public float speed = 5.0f; // Enemy movement speed
    public float attackRange = 1.0f; // Range at which the enemy can attack the player
    public float chaseRange = 10.0f; // Range at which the enemy starts chasing the player
    public float roamRange = 5.0f; // Range at which the enemy will roam

    private Transform player; // Reference to the player's transform
    private NavMeshAgent agent; // Reference to the NavMeshAgent component
    public Animator animator; // Reference to the Animator component

    public bool isChasing = false; // Is the enemy currently chasing the player?
    public bool isAttacking = false; // Is the enemy currently attacking the player?
    public bool isRoaming = true; // Is the enemy currently roaming?

    private Vector3 roamTarget; // Target position for roaming

    void Start()
    {
        // Get the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Get the NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();

        // Get the Animator component
        animator = GetComponent<Animator>();

        // Set the NavMeshAgent's speed
        agent.speed = speed;

        // Initialize roam target
        roamTarget = transform.position;
    }

    void Update()
    {
        // Calculate the distance between the enemy and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // If the player is within the chase range, start chasing
        if (distance <= chaseRange &&!isChasing)
        {
            isChasing = true;
            isRoaming = false;
            ChasePlayer();
        }
        // If the player is not within the chase range, stop chasing
        else if (distance > chaseRange && isChasing)
        {
            isChasing = false;
            isRoaming = true;
            StopChasing();
        }

        // If the player is within the attack range, attack
        if (distance <= attackRange &&!isAttacking)
        {
            isAttacking = true;
            AttackPlayer();
        }
        // If the player is not within the attack range, stop attacking
        else if (distance > attackRange && isAttacking)
        {
            isAttacking = false;
            StopAttacking();
        }

        // If the enemy is not chasing or attacking, roam
        if (isRoaming)
        {
            Roam();
        }
    }

    void ChasePlayer()
    {
        // Set the NavMeshAgent's destination to the player's position
        agent.SetDestination(player.position);

        // Play the chasing animation
        animator.SetBool("IsChasing", true);
    }

    void StopChasing()
    {
        // Stop the NavMeshAgent
        agent.isStopped = true;

        // Stop the chasing animation
        animator.SetBool("IsChasing", false);
    }

    void AttackPlayer()
    {
        // Play the attacking animation
        animator.SetBool("IsAttacking", true);
    }

    void StopAttacking()
    {
        // Stop the attacking animation
        animator.SetBool("IsAttacking", false);
    }

    void Roam()
    {
        // Calculate a new roam target if the enemy has reached the current target
        if (Vector3.Distance(transform.position, roamTarget) < 0.5f)
        {
            roamTarget = transform.position + new Vector3(Random.Range(-roamRange, roamRange), 0, Random.Range(-roamRange, roamRange));
        }

        // Set the NavMeshAgent's destination to the roam target
        agent.SetDestination(roamTarget);

        // Play the roaming animation
        animator.SetBool("IsRoaming", true);
    }

    void StopRoaming()
    {
        // Stop the NavMeshAgent
        agent.isStopped = true;

        // Stop the roaming animation
        animator.SetBool("IsRoaming", false);
    }
}
