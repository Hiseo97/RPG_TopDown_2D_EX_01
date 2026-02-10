using Unity.VisualScripting.FullSerializer;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.LightTransport;
using UnityEngine.Rendering.Universal.Internal;

public class EnemySight : MonoBehaviour
{
    private CircleCollider2D sightCheckArea;
    [SerializeField] int firstSight;
    public Vector2 forward;
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
        sightCheckArea = GetComponent<CircleCollider2D>();
        movement = GetComponent<EnemyMovement>();
        if (!eyes) eyes = transform;
    }

    void Start()
    {
        sightCheckArea.radius = viewDistance + 2;
        forward = AngToDir (firstSight);
        Debug.Log(forward);
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
            Debug.Log("콜라이더 작용 중");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == target)
        {
            _hasCandidate = false;
            target = null;
            Detected = false;
            Debug.Log("콜라이더 작용 해제");
        }
    }

    bool CheckVision()
    {
        if (!target) return false;  

        Vector2 origin = transform.position;
        Vector2 toPlayer = (Vector2)target.position - origin;

        float distSqr = toPlayer.sqrMagnitude;
        if (distSqr > viewDistance * viewDistance) return false;

        float dist = Mathf.Sqrt(distSqr);

        if (Vector2.Angle(forward, toPlayer) > viewAngle * 0.5f) return false;

        Vector2 dir = toPlayer / dist;

        int mask = playerMask | obstacleMask;
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dist, mask);

        if (!hit.collider) return false;

        forward = toPlayer;
        return ((1 << hit.collider.gameObject.layer) & playerMask) != 0;
    }

    Vector2 AngToDir(float angleDeg)
    {
    float rad = -(angleDeg-90f) * Mathf.Deg2Rad;
    return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3 origin = transform.position;

        Vector2 f2 = forward;
        Vector3 f3 = f2.sqrMagnitude > 0.0001f ? (Vector3)f2.normalized : transform.right;

        Gizmos.DrawWireSphere(origin, viewDistance);

        float half = viewAngle * 0.5f;
        Quaternion leftRot = Quaternion.Euler(0, 0, -half);
        Quaternion rightRot = Quaternion.Euler(0, 0, half);

        Gizmos.DrawLine(origin, origin + (leftRot * f3) * viewDistance);
        Gizmos.DrawLine(origin, origin + (rightRot * f3) * viewDistance);
    }
#endif
}
