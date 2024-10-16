/*using UnityEngine;

public class SlopeMovement : MonoBehaviour
{
    [SerializeField] private float baseSlideSpeed = 10f;
    [SerializeField] private float maxSlideSpeed = 20f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb2d;
    private Collider2D coll2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        bool isGrounded = GroundCheckUtility.IsGrounded(coll2d, groundLayer);
        if (isGrounded && IsOnSlope())
        {
            // 슬로프에서 미끄러지기
            Vector2 slideDirection = new Vector2(-transform.up.x, -transform.up.y).normalized; // 기울어진 방향으로 슬라이드
            float slideSpeed = Mathf.Lerp(baseSlideSpeed, maxSlideSpeed, 1f); // 속도 조정
            rb2d.linearVelocity = new Vector2(slideDirection.x * slideSpeed, slideDirection.y * slideSpeed); // 수직 속도 유지
        }
    }

    private bool IsOnSlope()
    {
        // Collider2D의 접촉 정보를 가져와서 기울어진 바닥인지 확인
        ContactPoint2D[] contacts = new ContactPoint2D[10];
        int contactCount = coll2d.GetContacts(contacts);

        for (int i = 0; i < contactCount; i++)
        {
            // 접촉 점의 법선 벡터가 수직이 아닐 경우 기울어진 바닥으로 판단
            if (Vector2.Dot(contacts[i].normal, Vector2.up) < 0.9962f)
            {
                return true;
            }
        }
        return false;
    }
}*/