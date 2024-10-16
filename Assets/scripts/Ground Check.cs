using UnityEngine;

public static class GroundCheckUtility
{
    // 공용으로 사용할 IsGrounded 함수
    public static bool IsGrounded(Collider2D collider, LayerMask groundLayer, float extraHeight = 0.05f)
    {
        Vector2 boxSize = new Vector2(collider.bounds.size.x * 0.9f, collider.bounds.size.y + extraHeight);
        Vector2 boxCenter = new Vector2(collider.bounds.center.x, collider.bounds.center.y - 0.1f);

        // 바닥에 닿는지 확인
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.down, extraHeight, groundLayer);

        // Gizmos로 박스를 그리기 위한 네 모서리 좌표 계산
        Vector2 topLeft = new Vector2(boxCenter.x - boxSize.x / 2, boxCenter.y + boxSize.y / 2);
        Vector2 topRight = new Vector2(boxCenter.x + boxSize.x / 2, boxCenter.y + boxSize.y / 2);
        Vector2 bottomLeft = new Vector2(boxCenter.x - boxSize.x / 2, boxCenter.y - boxSize.y / 2);
        Vector2 bottomRight = new Vector2(boxCenter.x + boxSize.x / 2, boxCenter.y - boxSize.y / 2);

        // 네 개의 모서리를 연결해서 박스 그리기
        Debug.DrawLine(topLeft, topRight, Color.green);
        Debug.DrawLine(topRight, bottomRight, Color.green);
        Debug.DrawLine(bottomRight, bottomLeft, Color.green);
        Debug.DrawLine(bottomLeft, topLeft, Color.green);

        // 바닥에 닿으면 true 반환
        return raycastHit.collider != null;
    }
}
