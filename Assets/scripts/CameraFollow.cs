using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform을 할당
    public Vector3 offset;    // 카메라와 플레이어 사이의 고정된 거리

    void Update()
    {
        // 카메라의 새로운 위치 계산
        float desiredX = player.position.x + offset.x;
        float desiredY = player.position.y + offset.y + 3f;

        // x축과 y축 제한 적용
        desiredY = Mathf.Clamp(desiredY, -2f, float.MaxValue); // y축은 -2 아래로 내려가지 않도록
        desiredX = Mathf.Clamp(desiredX, -4.5f, 8f); // x축은 -4.5 미만, 8 이상으로 제한

        // 카메라의 위치 업데이트
        transform.position = new Vector3(desiredX, desiredY, offset.z);
    }
}
