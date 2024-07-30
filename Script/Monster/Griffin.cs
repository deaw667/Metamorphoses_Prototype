using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GriffinState { Idle, Patrol, Chase, Attack }
public class Griffin : MonoBehaviour
{
    public GriffinState currentState { get; private set; }
    public GameObject chargeUpRadius;
    public int EnemySoundIndex1;
    public int EnemySoundIndex2;
    public int EnemySoundIndex3;
    public int EnemySoundIndex4;
    public int EnemySoundIndex5;

    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 5.0f;
    public float diveSpeed = 10.0f;
    public float attackRange = 1.0f;
    public float DamageRange = 1.0f;
    public float attackCooldown = 2.0f;
    public float chaseRange = 5.0f;
    public float randomDirectionTimer = 0.0f;
    public float randomStateTimer = 5.0f;
    public int AttackDMG;
    private Vector2 randomDirection;
    public bool isCharging;
    public bool isStopOnAttacking;
    public float EnemySoundRange;

    public float scaleX;
    public float scaleY;
    public float scaleZ;
    [ SerializeField ]
    private Vector2 PlayerLastSeen;
    

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    public float attackTimer = 0.0f;
    public bool isAttackAnimationPlaying = false;
    public bool isRangeAnimationPlaying = false;
    private SoundEffectSystem soundEffectSystem;

    [SerializeField]
    private AudioClip[] EnemySoundEffects; // Array of sound effects
    private List<AudioSource> EnemyAudioSources = new List<AudioSource>(); // List of AudioSource components

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
        currentState = GriffinState.Patrol;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        // Create multiple AudioSource components
        for (int i = 0; i < 5; i++) // Create 5 AudioSource components by default
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            EnemyAudioSources.Add(audioSource);
        }

        // Start debugging currentState every 2 seconds
        //InvokeRepeating("DebugCurrentState", 2.0f, 2.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case GriffinState.Idle:
                IdleState();
                break;
            case GriffinState.Patrol:
                PatrolState();
                break;
            case GriffinState.Chase:
                ChaseState();
                break;
            case GriffinState.Attack:
                AttackState();
                break;
        }
    }

    public void SavePlayerPosition()
    {
        PlayerLastSeen = player.position;
    }

    private void ChasePlayerLastSeen()
    {
        Vector2 direction = (PlayerLastSeen - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector2 targetPosition = PlayerLastSeen + direction * 2f;
        rb.velocity = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized * diveSpeed;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        }
    }

    private void IdleState()
    {
        animator.SetInteger("PlayerAnim", 1);

        if (randomStateTimer <= 0.0f)
        {
            randomStateTimer = Random.Range(4.0f, 10.0f);
            animator.SetInteger("PlayerAnim", 0);
            currentState = GriffinState.Patrol;
        }
        else
        {
            randomStateTimer -= Time.deltaTime;
        }

        // Code for idle state, e.g. waiting for player to come close
        if (Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            animator.SetInteger("PlayerAnim", 6);
            currentState = GriffinState.Chase;
        }

        // Set velocity to zero to prevent movement
        rb.velocity = Vector2.zero;
    }

    private void ChasePlayer()
    {
        // Move towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
    }


    private void StopMoving()
    {
        rb.velocity = Vector2.zero;
    }

    private void OnSoundRangeOn(int soundint)
    {
        if (Vector2.Distance(transform.position, player.position) < EnemySoundRange)
        {
            PlayEnemySoundEffect(soundint);
        }
    }

    
    private void OnSoundEffectOn(int soundint)
    {
        PlayEnemySoundEffect(soundint);
    }


    private void PatrolState()
    {
        animator.SetInteger("PlayerAnim", 0);

        if (randomDirectionTimer <= 0.0f)
        {
            randomDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            randomDirectionTimer = Random.Range(2.0f, 6.0f);
        }
        else
        {
            randomDirectionTimer -= Time.deltaTime;
        }

        if (randomStateTimer <= 0.0f)
        {
            randomStateTimer = Random.Range(4.0f, 10.0f);
            animator.SetInteger("PlayerAnim", 1);
            currentState = GriffinState.Idle;
        }
        else
        {
            randomStateTimer -= Time.deltaTime;
        }

        rb.velocity = randomDirection * patrolSpeed;

        // Check if the player is within chase range
        if (Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            animator.SetInteger("PlayerAnim", 2);
            currentState = GriffinState.Chase;
        }

        // Flip the sprite based on the direction of movement
        if (randomDirection.x > 0)
        {
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
        else if (randomDirection.x < 0)
        {
            transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        }
    }


    public void PlayEnemySoundEffect(int soundEffectIndex)
    {
        // Check if the sound effect index is valid
        if (soundEffectIndex >= 0 && soundEffectIndex < EnemySoundEffects.Length)
        {
            // Find an available AudioSource component
            foreach (AudioSource audioSource in EnemyAudioSources)
            {
                if (!audioSource.isPlaying)
                {
                    // Play the sound effect
                    audioSource.clip = EnemySoundEffects[soundEffectIndex];
                    audioSource.Play();
                    return;
                }
            }

            // If all AudioSource components are busy, create a new one
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.clip = EnemySoundEffects[soundEffectIndex];
            newAudioSource.Play();
            EnemyAudioSources.Add(newAudioSource);
        }
        else
        {
            Debug.LogError("Invalid sound effect index");
        }
    }


    private void ChaseState()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;

        // Code for chase state, e.g. following the player
        ChasePlayer();
        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            //AttackPlayer();
            //animator.SetInteger("PlayerAnim", 5);
            currentState = GriffinState.Attack;
        }
        else if (Vector2.Distance(transform.position, player.position) > chaseRange)
        {
            animator.SetInteger("PlayerAnim", 3);
            currentState = GriffinState.Patrol;
        }

        // Flip the sprite based on the direction of movement
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
        }
    }


    private void AttackState()
    {
        attackTimer += Time.deltaTime;


        if (Vector2.Distance(transform.position, player.position) > attackRange && !isAttackAnimationPlaying)
        {
            animator.SetInteger("PlayerAnim", 4);
            currentState = GriffinState.Chase;
        }


        if (attackTimer >= attackCooldown)
        {
            if (Vector2.Distance(transform.position, player.position) < attackRange && !isAttackAnimationPlaying)
            {
                animator.SetInteger("PlayerAnim", 5);
                isRangeAnimationPlaying = true;
                isAttackAnimationPlaying = true;
                attackTimer = 0.0f;
            }
            else if (Vector2.Distance(transform.position, player.position) < attackRange && isAttackAnimationPlaying)
            {
                animator.SetInteger("PlayerAnim", 5);
                isAttackAnimationPlaying = true;
                attackTimer = 0.0f;
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }
    }

    // Add an animation event to the attack animation to set isAttackAnimationPlaying to false when the animation finishes
    public void OnAttackAnimationFinished()
    {
        //chargeUpRadius.transform.localScale = new Vector3(0, 0, 0); // Reset chargeUpRadius
        isAttackAnimationPlaying = false;
    }

    public void OnRangeAnimationFinished()
    {
        //chargeUpRadius.transform.localScale = new Vector3(0, 0, 0); // Reset chargeUpRadius
        isRangeAnimationPlaying = false;
    }


    private void OnLeftFootWalked()
    {
        if (Vector2.Distance(transform.position, player.position) < EnemySoundRange)
        {
            PlayEnemySoundEffect(0);
        }
    }

    private void OnRightFootWalked()
    {
        if (Vector2.Distance(transform.position, player.position) < EnemySoundRange)
        {
            PlayEnemySoundEffect(1);
        }
    }

    private void AttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < DamageRange)
        {
            //AttackPlayer();
            //animator.SetInteger("PlayerAnim", 5);
            PlayerHealth.instance.TakeDamage(AttackDMG);
            currentState = GriffinState.Attack;
        }

        //OnAttackAnimationFinished();
        // Attack the player
    }

}
