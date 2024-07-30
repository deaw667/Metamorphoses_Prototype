using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Idle, Patrol, Chase, Attack }

public class ProtypeEnemy : MonoBehaviour
{
    public EnemyState currentState { get; private set; }
    public GameObject chargeUpRadius;
    public float patrolSpeed = 2.0f;
    public float chaseSpeed = 5.0f;
    public float attackRange = 1.0f;
    public float DamageRange = 1.0f;
    public float DamageRange2 = 3.0f;
    public float attackCooldown = 2.0f;
    public float chaseRange = 5.0f;
    public float randomDirectionTimer = 0.0f;
    public float randomStateTimer = 5.0f;
    public int AttackDMG;
    private Vector2 randomDirection;
    public bool isCharging;
    public bool isStopOnAttacking;
    public float EnemySoundRange;
    public bool isBoss;

    public GameObject Bullet;
    public float BulletSpeed;

    public int EnemySoundIndex1;
    public int EnemySoundIndex2;
    public int EnemySoundIndex3;
    public int EnemySoundIndex4;
    public int EnemySoundIndex5;

    public int AttackStyle;


    public float scaleX;
    public float scaleY;
    public float scaleZ;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;
    public float attackTimer = 0.0f;
    public bool isAttackAnimationPlaying = false;
    public bool isRangeAnimationPlaying = false;
    public bool RandomedStyle = false;
    private SoundEffectSystem soundEffectSystem;

    [SerializeField]
    private AudioClip[] EnemySoundEffects; // Array of sound effects
    private List<AudioSource> EnemyAudioSources = new List<AudioSource>(); // List of AudioSource components

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentState = EnemyState.Patrol;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        for (int i = 0; i < 5; i++) // Create 5 AudioSource components by default
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            EnemyAudioSources.Add(audioSource);
        }

    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.Patrol:
                PatrolState();
                break;
            case EnemyState.Chase:
                ChaseState();
                break;
            case EnemyState.Attack:
                AttackState();
                break;
        }
    }

    private void IdleState()
    {
        animator.SetInteger("PlayerAnim", 1);

        if (randomStateTimer <= 0.0f)
        {
            randomStateTimer = Random.Range(4.0f, 10.0f);
            animator.SetInteger("PlayerAnim", 0);
            currentState = EnemyState.Patrol;
        }
        else
        {
            randomStateTimer -= Time.deltaTime;
        }

        // Code for idle state, e.g. waiting for player to come close
        if (Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            animator.SetInteger("PlayerAnim", 6);
            currentState = EnemyState.Chase;
        }

        // Set velocity to zero to prevent movement
        rb.velocity = Vector2.zero;
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
            currentState = EnemyState.Idle;
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
            currentState = EnemyState.Chase;
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


    private void ChaseState()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;

        ChasePlayer();
        if (Vector2.Distance(transform.position, player.position) < attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else if (Vector2.Distance(transform.position, player.position) > chaseRange)
        {
            animator.SetInteger("PlayerAnim", 3);
            currentState = EnemyState.Patrol;
        }

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
        if (isStopOnAttacking)
        {
            rb.velocity = Vector2.zero;
        }

        if (Vector2.Distance(transform.position, player.position) > attackRange && !isAttackAnimationPlaying)
        {
            animator.SetInteger("PlayerAnim", 4);
            currentState = EnemyState.Chase;
        }
        
        if (isRangeAnimationPlaying && isCharging)
        {
            // Increase the chargeUpRadius
            chargeUpRadius.transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);
        }

        if(attackTimer < 0)
        {
            attackTimer = 0;
        }


        if (attackTimer >= attackCooldown && !isBoss)
        {
            if (Vector2.Distance(transform.position, player.position) < attackRange && !isAttackAnimationPlaying)
            {
                // Start the attack animation
                animator.SetInteger("PlayerAnim", 5);
                isRangeAnimationPlaying = true;
                isAttackAnimationPlaying = true;
                attackTimer = 0.0f;
            }
            else if (Vector2.Distance(transform.position, player.position) < attackRange && isAttackAnimationPlaying)
            {
                // Start the attack animation
                animator.SetInteger("PlayerAnim", 5);
                isAttackAnimationPlaying = true;
                attackTimer = 0.0f;
            }
        }
        else
        {
            attackTimer -= Time.deltaTime;
        }

        
        if (isBoss && !RandomedStyle)
        {
            AttackStyle = Random.Range(0, 2);
            RandomedStyle = true;
        }

        if (attackTimer >= attackCooldown && isBoss)
        {
            if (Vector2.Distance(transform.position, player.position) < attackRange && !isAttackAnimationPlaying)
            {
                if (AttackStyle == 0)
                {
                    animator.SetInteger("PlayerAnim", 5);
                    isRangeAnimationPlaying = true;
                    isAttackAnimationPlaying = true;
                    attackTimer = 0.0f;
                }
                else if(AttackStyle == 1)
                {
                    animator.SetInteger("PlayerAnim", 8);
                    isRangeAnimationPlaying = true;
                    isAttackAnimationPlaying = true;
                    attackTimer = 0.0f;
                }
            }
            else if (Vector2.Distance(transform.position, player.position) < attackRange && isAttackAnimationPlaying)
            {

                if (AttackStyle == 0)
                {
                    animator.SetInteger("PlayerAnim", 5);
                    isRangeAnimationPlaying = true;
                    isAttackAnimationPlaying = true;
                    attackTimer = 0.0f;
                }
                else if(AttackStyle == 1)
                {
                    animator.SetInteger("PlayerAnim", 8);
                    isRangeAnimationPlaying = true;
                    isAttackAnimationPlaying = true;
                    attackTimer = 0.0f;
                }
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
        chargeUpRadius.transform.localScale = new Vector3(0, 0, 0); // Reset chargeUpRadius
        isAttackAnimationPlaying = false;
        RandomedStyle = false;
    }

    public void OnRangeAnimationFinished()
    {
        isRangeAnimationPlaying = false;
    }

    private void PatrolWaypoint()
    {
 
    }

    private void ChasePlayer()
    {
        // Move towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * chaseSpeed;
    }

    private void AttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < DamageRange)
        {
            PlayerHealth.instance.TakeDamage(AttackDMG);
            currentState = EnemyState.Attack;
        }
    }

    private void SecondStyleAttackPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) < DamageRange)
        {
            PlayerHealth.instance.TakeDamage(AttackDMG);
            currentState = EnemyState.Attack;
        }
    }

    public void EnemyShooting() 
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

        GameObject newBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        rb.velocity = targetDirection * BulletSpeed;
    }

    private void DebugCurrentState()
    {
        Debug.Log("Current State: " + currentState);
    }
}