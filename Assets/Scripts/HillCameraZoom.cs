using UnityEngine;
using Unity.Cinemachine;

public class HillCameraZoom : MonoBehaviour
{
    public CinemachineCamera vcam;
    public float normalSize;
    public float zoomSpeed;

    float targetSize;

    void Start()
    {
        targetSize = normalSize;
    }

    void Update()
    {
        if (!vcam) return;

        vcam.Lens.OrthographicSize = Mathf.Lerp(
            vcam.Lens.OrthographicSize,
            targetSize,
            Time.deltaTime * zoomSpeed
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HillZone hill = other.GetComponent<HillZone>();
        if (hill)
            targetSize = hill.zoomSize;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Hill"))
            targetSize = normalSize;
    }
}
