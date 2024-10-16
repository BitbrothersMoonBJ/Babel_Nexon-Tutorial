/*플랫폼 매니저로 통합

using UnityEngine;

public class MoveWithPlatform : MonoBehaviour
{
    private Vector3 lastPlatformPosition;
    private bool isPlayerOnPlatform;
    private GameObject Player;

    void Start()
    {
        lastPlatformPosition = transform.position;  // 발판의 초기 위치 저장
        Player = GameObject.FindWithTag("Player");  // 'Player' 오브젝트를 찾음
    }

    void Update()
    {
        Vector3 platformMovement = transform.position - lastPlatformPosition;  // 발판의 이동 벡터 계산
        lastPlatformPosition = transform.position;  // 현재 위치 저장

        // 캐릭터가 발판 위에 있는지 확인
        if (isPlayerOnPlatform && Player != null)  // Player가 null이 아닌지 확인
        {
            Player.transform.position += platformMovement;  // 캐릭터 위치를 발판 이동량만큼 이동
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = true;  // 캐릭터가 발판 위에 있을 때
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;  // 캐릭터가 발판을 떠났을 때
        }
    }
}*/
