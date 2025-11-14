using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigid2d;
    private SpriteRenderer spriteRenderer;

    public Sprite spriteRight; // 오른쪽 이미지
    public Sprite spriteLeft;  // 왼쪽 이미지

    public float currentSpeed;
    public float walkSpeed = 5.0f;  // 기본 속도
    public float runSpeed = 8.0f;   // 쉬프트 누르면 빨라지는 속도
    public float slowSpeed = 3.0f;  // 컨트롤 누르면 느려지는 속도
    public float jumpForce = 7.0f;  // 점프 힘

    private bool isGrounded = false;
    public Transform groundCheck; // 바닥 감지 위치
    public float groundCheckDistance = 0.2f; // 바닥 감지 거리
    public LayerMask groundLayer; // 바닥 레이어

    private void Awake()
    {
        rigid2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // 플레이어를 저장된 위치로 이동
    void Start()
    {
        if (DataManager.instance != null)
        {
            Vector3 savedPos = DataManager.instance.GetPlayerPosition();
            if (savedPos != Vector3.zero)
            {
                transform.position = savedPos;
            }
        }
    }


    private void Update()
    {
        // a, left 키를 누르면 -1, d, right키를 누르면 +1, 아무키도 안누르면 0
        float x = Input.GetAxisRaw("Horizontal");
        Vector3 moveDirection = new Vector3(x, 0, 0);   // 이동 방향 설정

        // 쉬프트 키를 누르면 속도 증가, 컨트롤 키를 누르면 속도 감소
        currentSpeed = walkSpeed;
        if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = slowSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }


        // 오브젝트 속력 설정
        rigid2d.velocity = new Vector2(moveDirection.x * currentSpeed, rigid2d.velocity.y);

        // 방향에 따라 Sprite 변경
        if (x > 0)
        {
            spriteRenderer.sprite = spriteRight;
        }
        else if (x < 0)
        {
            spriteRenderer.sprite = spriteLeft;
        }

        // 바닥에 닿았을 때만 점프하도록 설정
        // 바닥 감지 - 플레이어 아래의 레이저가 바닥에 닿는지 아닌지 확인
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        // 디버그용 레이저 시각화
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, isGrounded ? Color.green : Color.red);

        // W키, 윗 방향키 중 하나라도 누르면 점프
        if ((Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rigid2d.velocity = new Vector2(rigid2d.velocity.x, jumpForce);
        }
    }
}
