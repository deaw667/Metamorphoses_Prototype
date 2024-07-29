using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State {
        Roaming
    }

    private State state;
    private EnemyPathfinding enemyPathfinding;
    public Collider2D AttackRange;
    public GameObject Bullet;
    public GameObject PlayerPosition;
    public float BulletSpeed;
    public bool CheckisChasing = false;
    public bool isAttacking = false;
    public bool canAttack = true;
    [SerializeField] private float attackRange = 5f;

    [SerializeField] private float AttackRadius;
    [SerializeField] private float attackCooldown;
    //[SerializeField] private bool StopWhileAttacking = false;

    private void Awake() 
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private IEnumerator RoamingRoutine() 
    {
        while (state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(3f);
        }
    }
    private void Start() 
    {
        StartCoroutine(RoamingRoutine());
    }
/*
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player") && AttackRange.IsTouching(other)) {
            Debug.Log("PlayerEnterArea");
            state = State.Chase;
            //PlayerHealth.instance.TakeDamage(1);
        }
    }

        
    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            Debug.Log("PlayerExitArea");
            state = State.Roaming;
        }
    } */

    private void GoingToAttacking()
    {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange && canAttack == true)
        {
            isAttacking = true;
            canAttack = false;
            Debug.Log("I will kill you");
            Attack();
            StartCoroutine(AttackCooldownRoutine());
        }
    }
    
    public void Attack() 
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;

        GameObject newBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        rb.velocity = targetDirection * BulletSpeed;
    }

    private Vector2 GetRoamingPosition() 
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private IEnumerator AttackCooldownRoutine() 
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


    private void Update() 
    {
        GoingToAttacking();
    }
}
