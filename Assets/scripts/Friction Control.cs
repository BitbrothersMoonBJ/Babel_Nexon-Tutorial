using UnityEngine;

public class FrictionControl : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D groundedMaterial; // 평평한 바닥에서의 물리 재질 (마찰력 1)
    [SerializeField] private PhysicsMaterial2D slopeMaterial;    // 기울어진 땅이나 경사면에서의 물리 재질 (마찰력 0)
    [SerializeField] private PhysicsMaterial2D airMaterial;      // 공중에 있을 때의 물리 재질 (마찰력 0)
    [SerializeField] private LayerMask groundLayer;              // 바닥 레이어 확인용

    private Rigidbody2D rb2d;
    private Collider2D coll2d;
    private Vector2 groundNormal;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<Collider2D>();

        if (rb2d == null || coll2d == null)
        {
            Debug.LogError("Rigidbody2D 또는 Collider2D가 'Player' 오브젝트에 없습니다!");
        }
    }

    void FixedUpdate()
    {
        bool isGrounded = GroundCheckUtility.IsGrounded(coll2d, groundLayer);

        // 바닥에 있는지 여부 확인
        if (isGrounded)
        {
            // 경사면 여부에 따라 마찰력 설정
            if (IsOnSlope())
            {
                coll2d.sharedMaterial = slopeMaterial; // 경사면에서 마찰력 0
            }
            else
            {
                coll2d.sharedMaterial = groundedMaterial; // 평평한 바닥에서 마찰력 1
            }
        }
        else
        {
            coll2d.sharedMaterial = airMaterial; // 공중에서 마찰력 0
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 바닥에 닿는 순간 잠시 Grounded Material 적용
            ApplyFriction(groundedMaterial);

            // 0.05초 후 다시 마찰력 재설정
            Invoke("ResetFriction", 0.05f);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 매 프레임마다 groundNormal 업데이트
            groundNormal = collision.contacts[0].normal;
        }
    }

    // 마찰력 재설정 함수
    private void ResetFriction()
    {
        bool isGrounded = GroundCheckUtility.IsGrounded(coll2d, groundLayer);

        if (isGrounded)
        {
            if (IsOnSlope())
            {
                ApplyFriction(slopeMaterial); // 경사면일 경우
            }
            else
            {
                ApplyFriction(groundedMaterial); // 평지일 경우
            }
        }
    }

    // 경사면 여부를 확인 (10도 이상일 때 경사면으로 간주)
    private bool IsOnSlope()
    {
        float slopeAngle = Vector2.Dot(groundNormal, Vector2.up);
        return slopeAngle < 0.9962f; // 5도 이상의 경사면
    }

    // 마찰력 적용 함수
    private void ApplyFriction(PhysicsMaterial2D material)
    {
        coll2d.sharedMaterial = material;
    }
}
