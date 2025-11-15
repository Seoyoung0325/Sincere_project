/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public float offsetY = 3.5f; // 카메라가 플레이어보다 살짝 위에 위치하도록

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null) return;

        transform.position = new Vector3(
            target.transform.position.x,
            target.transform.position.y + offsetY,
            transform.position.z
        );
    }
}*/
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

        // 따라갈 위치 계산 (오프셋 포함)
        float targetX = target.transform.position.x;
        float targetY = target.transform.position.y + offsetY;

        // 맵 범위 내로 제한
        float clampedX = Mathf.Clamp(targetX, minBounds.x + camWidth, maxBounds.x - camWidth);
        float clampedY = Mathf.Clamp(targetY, minBounds.y + camHeight, maxBounds.y - camHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}