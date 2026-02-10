using System;
using TreeEditor;
using UnityEditor;
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
    EnemyVision2D vision;
    [SerializeField] Transform player;
    [SerializeField] float speed = 3f;
    public Vector2 Forward { get; private set; } // 마지막 유효 방향(정규화)
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vision = GetComponent<EnemyVision2D>();
        anim = GetComponent<Animator>();

        Forward = transform.right; // 초기 정면
        ChangeState(EnemyState.Idle);

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
        if (vision.Detected && player)
        {
            
            if (Vector2.Distance(transform.position, player.transform.position) <= attackRange && attackCooldownTimer <= 0)
            {
                attackCooldownTimer = attackCooldown;
                ChangeState(EnemyState.Attacking);
            }

            else if (Vector2.Distance(transform.position, player.transform.position) > attackRange)
            {
                ChangeState(EnemyState.Chasing);
                if (player.position.x > transform.position.x && facingDirection == -1 ||
                    player.position.x < transform.position.x && facingDirection == 1)
                {
                    Flip();
                }
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
        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        Vector2 vel = dir * speed;
        if (dir.sqrMagnitude > 0.0001f)
            Forward = dir;
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