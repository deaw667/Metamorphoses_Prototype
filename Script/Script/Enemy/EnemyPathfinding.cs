using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 2f;

    #region singleton
    public static EnemyPathfinding instance;
    public float Defulatspeed;

    private void Start()
    {
        Defulatspeed = moveSpeed;
    }
    private void Awake() 
    {
        if (instance == null)
        instance = this;
        rb = GetComponent<Rigidbody2D>();
    }
    #endregion

    private Rigidbody2D rb;
    private Vector2 moveDir;

    private void FixedUpdate() 
    {
        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
    }

    public void UnFreezeEnemy()
    {
        moveSpeed = Defulatspeed;
    }

    public void FreezeEnemy()
    {
        moveSpeed = 0f;
    }

    public void MoveTo(Vector2 targetPosition) 
    {
        moveDir = targetPosition;
    }
}
