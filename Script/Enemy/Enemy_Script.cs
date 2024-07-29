using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    private Animator myAnim;
    private Transform target;
    public Transform homePos;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxRange;
    [SerializeField]
    private float minRange;

    void Start()
    {
        myAnim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if(Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position)>= minRange)
        {
            FollowPlayer();  
        }
        else if(Vector3.Distance(target.position, transform.position) >= maxRange )
        {
            GoHome();
        }
    }

    public void FollowPlayer()
    {
        myAnim.SetBool("isRunning",true);
        myAnim.SetFloat("X", (target.position.x - transform.position.x));
        myAnim.SetFloat("Y", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
    }

    public void GoHome()
    {
        myAnim.SetFloat("X", (homePos.position.x - transform.position.x));
        myAnim.SetFloat("Y", (homePos.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, homePos.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, homePos.position) == 0)
        {
            myAnim.SetBool("isRunning", false);
        }
    }

/*
public class Enemy_Script : MonoBehaviour
{
    private Animator myAnim;
    private Transform target;
    public Transform homePos;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxRange;
    [SerializeField]
    private float minRange;

    void Start()
    {
        myAnim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if(Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position)>= minRange)
        {
            FollowPlayer();  
        }
        else if(Vector3.Distance(target.position, transform.position) >= maxRange )
        {
            GoHome();
        }
    }

    public void FollowPlayer()
    {
        myAnim.SetBool("isRunning",true);
        myAnim.SetFloat("X", (target.position.x - transform.position.x));
        myAnim.SetFloat("Y", (target.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed*Time.deltaTime);
    }

    public void GoHome()
    {
        myAnim.SetFloat("X", (homePos.position.x - transform.position.x));
        myAnim.SetFloat("Y", (homePos.position.y - transform.position.y));
        transform.position = Vector3.MoveTowards(transform.position, homePos.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, homePos.position) == 0)
        {
            myAnim.SetBool("isRunning", false);
        }
    }






    */
/*    public float speed;
    public float checkRadius;
    public float attackRadius;

    public bool shouldRotate;

    public LayerMask whatIsPlayer;

    private Transform target;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    public Vector3  dir;

    private bool isInChaseRange;
    private bool isInAttackRange;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        anim.SetBool("isRunning", isInChaseRange);

        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        isInChaseRange = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

        dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;
        if(shouldRotate)
        {
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
        }
    }

    private void FixedUpdate()
    {
        if(isInChaseRange && !isInAttackRange)
        {
            MoveCharacter(movement);
        }
        if(isInAttackRange)
        {
            rb.velocity = Vector2.zero;
        }
    }
    
    private void MoveCharacter(Vector2 dir)
    {
        rb.MovePosition((Vector2)transform.position * (dir * speed * Time.deltaTime));
    }*/
}