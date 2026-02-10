using System;
using TreeEditor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.XR;

public class EnemyMovement : MonoBehaviour
{
    public EnemyState enemyState, newState;
    public float attackRange = 2;
    public float attackCooldown = 2;
    private float attackCooldownTimer;
    private Rigidbody2D rb;
    private int facingDirection = 1;
    EnemySight Sight;
    [SerializeField] Transform player;
    [SerializeField] float speed = 2f;
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Sight = GetComponentInChildren<EnemySight>();
        anim = GetComponent<Animator>();
        ChangeState(EnemyState.Idle);

    }

    void Update()
    {
        if (Sight.forward.x >= 0 && facingDirection == -1 || Sight.forward.x < 0 && facingDirection == 1)
            {
                Flip();
            }
    }
    void FixedUpdate()
    {
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        CheckForPlayer();
    }

    void CheckForPlayer()
    {
        if (Sight.Detected && player)
        {
            float EnemyPlayerDistSqr = (transform.position - player.transform.position).sqrMagnitude;
            if (attackCooldownTimer <= 0 && EnemyPlayerDistSqr <= attackRange*attackRange)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }

            else if (EnemyPlayerDistSqr > attackRange*attackRange)
            {
                ChangeState(EnemyState.Chasing);
                Chase ();
                Debug.Log("Chase");
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            ChangeState(EnemyState.Idle);
        }

    }


    void Chase()
    {
        Vector2 dir = Sight.forward.normalized;
        Vector2 vel = dir * speed;
        rb.linearVelocity = vel;
    }

    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    void ChangeState(EnemyState newState)
    {
        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", false);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", false);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", false);

        enemyState = newState;

        if (enemyState == EnemyState.Idle)
            anim.SetBool("isIdle", true);
        else if (enemyState == EnemyState.Chasing)
            anim.SetBool("isChasing", true);
        else if (enemyState == EnemyState.Attacking)
            anim.SetBool("isAttacking", true);
    }
}


public enum EnemyState
{
    Idle,
    Chasing,
    Attacking
}