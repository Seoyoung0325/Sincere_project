using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float offsetY = 1.5f;

    public Vector2 minBounds; // 맵의 좌하단 (최소 x, y)
    public Vector2 maxBounds; // 맵의 우상단 (최대 x, y)

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // 카메라 크기 계산
        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        // 따라갈 위치 계산
        float targetX = target.transform.position.x;
        float targetY = target.transform.position.y + offsetY;

        // 카메라 이동 범위를 맵 범위 내로 제한
        float clampedX = Mathf.Clamp(targetX, minBounds.x + camWidth, maxBounds.x - camWidth);
        float clampedY = Mathf.Clamp(targetY, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}