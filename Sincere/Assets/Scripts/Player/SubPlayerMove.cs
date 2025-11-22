using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPlayerMove : MonoBehaviour
{
    public Transform target;                  // 따라갈 플레이어
    public float followDistance = 1f;       // 플레이어와의 거리 유지
    public float followSpeed = 5f;            // 따라가는 속도

    public float obstacleCheckDistance = 1.0f; // 장애물 감지 거리
    public LayerMask obstacleLayer;           // 장애물 레이어
    public float jumpForce = 7f;              // 점프 힘

    public Transform groundCheck;             // 펫의 바닥 감지 위치
    public float groundCheckRadius = 0.2f;    // 바닥 감지 범위
    public LayerMask groundLayer;             // 바닥 레이어

    public PlayerMove playerMove;             // 플레이어의 스크립트 참조
    public Transform obstacleCheckPoint; // 장애물 감지용 위치

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (target == null || playerMove == null) return;

        // 플레이어가 움직이고 있는지 확인
        float playerVelocityX = playerMove.GetComponent<Rigidbody2D>().velocity.x;

        if (Mathf.Abs(playerVelocityX) != 0f)
        {
            float direction = Mathf.Sign(playerVelocityX); // 1: 오른쪽, -1: 왼쪽
            float targetX = target.position.x - direction * followDistance;
            float distanceToTarget = targetX - transform.position.x;

            float velocityX = distanceToTarget * followSpeed;

            rb.velocity = new Vector2(velocityX, rb.velocity.y);

            // 장애물 감지
            Vector2 rayOrigin = obstacleCheckPoint.position;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * Mathf.Sign(rb.velocity.x), obstacleCheckDistance, obstacleLayer);

            // 장애물 있을 때만 점프
            if (hit.collider != null && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
        else
        {
            // 플레이어가 멈췄다면 펫도 멈춤
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // 너무 멀어졌을 때 순간이동 (비상용)
        float distanceToPlayer = Mathf.Abs(target.position.x - transform.position.x);
        if (distanceToPlayer > 7f)
        {
            transform.position = target.position + Vector3.left * followDistance;
        }
    }

    // 펫의 바닥 감지
    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}