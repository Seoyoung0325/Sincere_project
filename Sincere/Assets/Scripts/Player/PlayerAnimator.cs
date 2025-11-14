using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigid2d;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 현재 x축 속도 가져오기
        float moveX = rigid2d.velocity.x;

        // 움직임 여부 판단 (속도가 거의 0이면 정지로 간주)
        bool isMoving = Mathf.Abs(moveX) > 0f;

        // Animator 파라미터 설정
        animator.SetFloat("MoveX", moveX);
        animator.SetBool("isMoving", isMoving);
    }
}