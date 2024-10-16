using UnityEngine;
using System.Collections; 

public class Jumping : MonoBehaviour
{
    [SerializeField] private float jumpForce = 12f;  // 기본 점프 힘
    [SerializeField] public float MinHorizontaljumpForce;
    [SerializeField] public float MaxHorizontaljumpForce;
    [SerializeField] private float oscillationSpeed = 2f; // 점프 거리의 변동 속도
    [SerializeField] private LayerMask groundLayer; // 바닥을 확인할 레이어 설정
    [SerializeField] private Animator animator;  // 애니메이터 컴포넌트

    private float chargeTimer = 0f; 
    private float oscillation;
    private float jumpStartTime;
    private bool canJump = true;
    private bool isJumping = false; // 점프 중인지 여부
    private bool isFacingRight = true; // 캐릭터가 오른쪽을 보고 있는지 여부
    private bool isGrounded; // 캐릭터가 바닥에 있는지 여부
    public float currentHorizontalForce; // 현재 수평 점프 힘
    private Rigidbody2D rb2d; // 2D 물리 적용을 위한 Rigidbody2D
    private Collider2D coll2d;
    private Vector2 groundNormal; // 바닥의 normal 벡터

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<Collider2D>();

        if (rb2d == null || coll2d == null)
        {
            Debug.LogError("Rigidbody2D 또는 Collider2D가 'Player' 오브젝트에 없습니다!");
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        if (rb2d == null || coll2d == null) return;

        // 캐릭터가 땅에 있는지 확인
        bool isGrounded = GroundCheckUtility.IsGrounded(coll2d, groundLayer);

        // 좌우 방향키 입력 감지 (캐릭터 방향 변경)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        FlipDirection(horizontalInput);

        // 점프 상태 처리: 바닥에 있지 않으면 더 이상 점프할 수 없음
        if (!isGrounded && isJumping)
        {
            return;
        }

        // 스페이스바를 누르고 있을 때 점프 거리 증가
        if (Input.GetKey(KeyCode.Space) && isGrounded && !isJumping && canJump)
        {
            chargeTimer += Time.deltaTime;
            ChargeJump(); // 점프 거리를 증가시킴
        }

        // 스페이스바를 떼면 점프 시작 (isJumping이 false일 때만 가능)
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded && !isJumping)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                jumpForce *= 1.3f;
                currentHorizontalForce = currentHorizontalForce / 1.5f;
                isJumping = true;
                Jump();
            }
            else
            {
                isJumping = true;
                Jump();
            }
            StartCoroutine(ResetJump());
        }

        // 점프 중이 아니고, 땅에 있을 때 Idle 상태로 전환
        if (isGrounded && isJumping)
        {
            isJumping = false;
            if (animator != null)
            {
                animator.SetBool("isJumping", true); // Idle 상태로 전환
            }
        }
    }
    private void ChargeJump()
    {
        oscillation = Mathf.Sin((chargeTimer * oscillationSpeed) - Mathf.PI / 2); // 사인파의 시작을 0으로 설정
        currentHorizontalForce = Mathf.Lerp(MinHorizontaljumpForce, MaxHorizontaljumpForce, (oscillation + 1f) / 2f);
    }

    private void Jump()
    {
        if (animator != null)
        {
            animator.SetBool("isJumping", true); // 점프 애니메이션 활성화
        }

        // 수평 힘 적용
        float horizontalForce = isFacingRight ? currentHorizontalForce : -currentHorizontalForce;
        Vector2 jumpDirection = new Vector2(horizontalForce, jumpForce);

        rb2d.AddForce(jumpDirection, ForceMode2D.Impulse); // 포물선을 그리며 점프

        // 점프 후 초기화
        jumpForce = 12f;
        currentHorizontalForce = 2f;
        oscillation = -1f;
        chargeTimer = 0f;
    }

    private void AdjustJumpAndHorizontalForce()
    {
        // 바닥의 normal을 기반으로 기울기 계산
        float slopeAngle = Vector2.Dot(groundNormal, Vector2.up);

        // 0도일 때는 jumpForce 그대로, 90도일 때는 0.5배
        jumpForce *= Mathf.Lerp(1f, 0.5f, 1 - slopeAngle); 

        // 0도일 때는 horizontalForce 그대로, 90도일 때는 2배
        currentHorizontalForce *= Mathf.Lerp(1f, 2f, 1 - slopeAngle);
    }

    // 좌우 방향 전환 처리
    private void FlipDirection(float horizontalInput)
    {
        if (horizontalInput > 0 && !isFacingRight)
        {
            FlipCharacter(); // 오른쪽으로 회전
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            FlipCharacter(); // 왼쪽으로 회전
        }
    }

    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f); // 캐릭터 회전
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Moving"))
        {
            isJumping = false;
            groundNormal = collision.contacts[0].normal; // groundNormal 업데이트

            if (animator != null)
            {
                animator.SetBool("isJumping", false); // Idle 상태로 돌아옴
            }

            if (collision.contacts[0].normal.x > 0.5f)
            {
                Vector2 bounceForce = new Vector2(currentHorizontalForce, 0f); // 수평 방향으로 힘을 가함
                rb2d.AddForce(bounceForce, ForceMode2D.Impulse); // 반발력 적용
            }else if(collision.contacts[0].normal.x < -0.5f)
            {
                Vector2 bounceForce = new Vector2(-currentHorizontalForce, 0f); // 수평 방향으로 힘을 가함
                rb2d.AddForce(bounceForce, ForceMode2D.Impulse); // 반발력 적용
            }

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Moving"))
        {
            groundNormal = collision.contacts[0].normal; // 지속적으로 groundNormal 업데이트
        }
    }

    private IEnumerator ResetJump()
    {
        canJump = false; // 일단 점프를 할 수 없도록 설정
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
        canJump = true; // 다시 점프 가능
    }
}
