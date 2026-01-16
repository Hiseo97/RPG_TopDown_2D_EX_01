using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    EnemyVision2D vision;

    [SerializeField] Transform player;
    [SerializeField] float speed = 3f;

    public Vector2 Forward { get; private set; } // 마지막 유효 방향(정규화)

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        vision = GetComponent<EnemyVision2D>();

        Forward = transform.right; // 초기 정면
    }

    void FixedUpdate()
    {
        Vector2 vel = Vector2.zero;

        if (vision.Detected && player)
        {
            Vector2 dir = ((Vector2)player.position - rb.position).normalized;
            vel = dir * speed;

            if (dir.sqrMagnitude > 0.0001f)
                Forward = dir; // 마지막 방향 갱신
        }

        rb.linearVelocity = vel;
    }
}