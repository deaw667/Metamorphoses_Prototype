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
}
