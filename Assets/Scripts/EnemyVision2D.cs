using UnityEngine;

public class EnemyVision2D : MonoBehaviour
{
    EnemyMovement movement;

    [Header("Precheck")]
    [SerializeField] LayerMask targetLayer;

    [Header("Vision")]
    [SerializeField] float viewDistance = 6f;
    [SerializeField, Range(0, 360)] float viewAngle = 90f;
    [SerializeField] Transform eyes;
    [SerializeField] Transform target;

    [Header("Block")]
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstacleMask;

    [Header("Tick")]
    [SerializeField] float checkInterval = 0.1f;

    public bool Detected { get; private set; }

    float _nextCheck;
    bool _hasCandidate;

    void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        if (!eyes) eyes = transform;
    }

    void Update()
    {
        if (!_hasCandidate)
        {
            Detected = false;
            return;
        }

        if (Time.time < _nextCheck) return;
        _nextCheck = Time.time + checkInterval;

        Detected = CheckVision();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            target = other.transform;
            _hasCandidate = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == target)
        {
            _hasCandidate = false;
            target = null;
            Detected = false;
        }
    }

    bool CheckVision()
    {
        if (!target) return false;

        Vector2 origin = eyes ? (Vector2)eyes.position : (Vector2)transform.position;
        Vector2 toPlayer = (Vector2)target.position - origin;

        float distSqr = toPlayer.sqrMagnitude;
        if (distSqr > viewDistance * viewDistance) return false;

        float dist = Mathf.Sqrt(distSqr);

        // ✅ 여기서만 movement.Forward 읽어옴 (복사 변수 제거)
        Vector2 forward = movement ? movement.Forward : (Vector2)transform.right;
        if (Vector2.Angle(forward, toPlayer) > viewAngle * 0.5f) return false;

        Vector2 dir = toPlayer / dist;

        int mask = playerMask | obstacleMask;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dist, mask);

        if (!hit.collider) return false;

        return ((1 << hit.collider.gameObject.layer) & playerMask) != 0;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 origin = eyes ? eyes.position : transform.position;

        Vector2 f2 = movement ? movement.Forward : (Vector2)transform.right;
        Vector3 forward = f2.sqrMagnitude > 0.0001f ? (Vector3)f2.normalized : transform.right;

        Gizmos.DrawWireSphere(origin, viewDistance);

        float half = viewAngle * 0.5f;
        Quaternion leftRot = Quaternion.Euler(0, 0, -half);
        Quaternion rightRot = Quaternion.Euler(0, 0, half);

        Gizmos.DrawLine(origin, origin + (leftRot * forward) * viewDistance);
        Gizmos.DrawLine(origin, origin + (rightRot * forward) * viewDistance);
    }
#endif
}
