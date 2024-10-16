using UnityEngine;

public class CharacterDirectionChange : MonoBehaviour
{
    private bool isFacingRight = true; // 캐릭터가 오른쪽을 보고 있는지 여부

    void Update()
    {
        // 좌우 방향키 입력 감지
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // 오른쪽 입력을 받으면 오른쪽으로 회전
        if (horizontalInput > 0 && !isFacingRight)
        {
            FlipCharacter(); // 오른쪽으로 회전
        }
        // 왼쪽 입력을 받으면 왼쪽으로 회전
        else if (horizontalInput < 0 && isFacingRight)
        {
            FlipCharacter(); // 왼쪽으로 회전
        }
    }

    // 캐릭터의 y축 회전을 반전하여 보는 방향을 바꾸는 함수
    private void FlipCharacter()
    {
        isFacingRight = !isFacingRight;

        // 캐릭터를 좌우로 반전시키기 위해 y축 회전 180도 변경
        transform.Rotate(0f, 180f, 0f);
    }
}
