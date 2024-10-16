using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class PlatformManager : MonoBehaviour
{
    private PlatformDataList.PlatformData platformData;
    public PlatformDataList platformDataList;  // PlatformDataList를 통해 발판 데이터를 관리
    private GameObject player;

    void Start()
    {
        // 'Player' 태그가 있는 오브젝트를 찾음
        player = GameObject.FindWithTag("Player");

        // PlatformDataList를 비동기적으로 Addressable을 통해 로드
        Addressables.LoadAssetAsync<PlatformDataList>("PlatformDataList").Completed += OnPlatformDataLoaded;
    }

    // PlatformDataList가 로드되면 실행되는 메서드
    void OnPlatformDataLoaded(AsyncOperationHandle<PlatformDataList> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            PlatformDataList loadedData = handle.Result;
            // platformData 설정
            platformData = loadedData.platformDatas[0];
            Debug.Log("platformData가 로드되었습니다.");
            
            // 이후 플랫폼 생성
            SpawnPlatforms();
        }
        else
        {
            Debug.LogError("PlatformData를 로드하는데 실패했습니다.");
        }
    }

    // 발판을 생성하는 메서드
    private void SpawnPlatforms()
    {
        foreach (var platformData in platformDataList.platformDatas)
        {
            // 발판을 생성
            GameObject platform = Instantiate(platformData.platformPrefab, platformData.startPoint, Quaternion.identity);

            // Moving 옵션이 체크된 경우
            if (platformData.moving)
            {
                StartCoroutine(MovePlatform(platform, platformData));
            }

            // Breakable 옵션이 체크된 경우
            if (platformData.breakable)
            {
                BreakablePlatform breakablePlatform = platform.AddComponent<BreakablePlatform>();
                breakablePlatform.Initialize(this);

                // 충돌 감지 및 부서지는 처리
                StartCoroutine(DetectPlatformCollision(platform, platformData));
            }
        }
    }

    // 발판이 움직이도록 하는 코루틴
    private IEnumerator MovePlatform(GameObject platform, PlatformDataList.PlatformData platformData)
    {
        Vector3 startPosition = platform.transform.position;
        Vector3 endPosition = startPosition + (Vector3)platformData.movementOffset;
        bool hasPlayerContacted = false;
        float moveTimer = 0f;  // 시간 누적을 위한 변수

        GameObject player = GameObject.FindWithTag("Player");  // 'Player' 오브젝트를 찾음
        Vector3 lastPlatformPosition = platform.transform.position;  // 발판의 마지막 위치를 저장

        while (true)
        {
            if (platformData.moveOnPlayerContact)
            {
                if (!hasPlayerContacted)
                {
                    Collider2D collider = platform.GetComponent<Collider2D>();
                    Vector2 boxCenter = new Vector2(collider.bounds.center.x, collider.bounds.max.y + 0.1f);
                    Vector2 boxSize = new Vector2(collider.bounds.size.x, 0.2f);

                    RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Player"));

                    if (hit.collider != null)
                    {
                        hasPlayerContacted = true;
                        startPosition = platform.transform.position;  // 충돌이 일어난 후 현재 위치를 시작점으로 설정
                        endPosition = startPosition + (Vector3)platformData.movementOffset;  // 끝점 재설정
                    }
                }

                if (hasPlayerContacted)
                {
                    // 이동 시간 누적
                    moveTimer += Time.deltaTime * platformData.speed;  // deltaTime을 사용해 시간이 누적되도록 설정
                    float time = Mathf.PingPong(moveTimer, 1);  // PingPong은 0에서 1 사이의 값으로 왕복
                    platform.transform.position = Vector3.Lerp(startPosition, endPosition, time);

                    // 발판이 움직일 때 캐릭터도 같이 이동
                    MovePlayerWithPlatform(platform, player, ref lastPlatformPosition);
                }
            }
            else
            {
                // 캐릭터 접촉 여부와 상관없이 계속 움직임
                moveTimer += Time.deltaTime * platformData.speed;
                float time = Mathf.PingPong(moveTimer, 1);
                platform.transform.position = Vector3.Lerp(startPosition, endPosition, time);

                // 발판이 움직일 때 캐릭터도 같이 이동
                MovePlayerWithPlatform(platform, player, ref lastPlatformPosition);
            }

            yield return null;
        }
    }

    // 발판이 움직일 때 캐릭터도 함께 움직이도록 처리하는 함수
    private void MovePlayerWithPlatform(GameObject platform, GameObject player, ref Vector3 lastPlatformPosition)
    {
        if (player != null && IsPlayerOnPlatform(platform))
        {
            // 발판의 이동량을 계산
            Vector3 platformMovement = platform.transform.position - lastPlatformPosition;

            // 캐릭터의 위치를 발판 이동량만큼 이동
            player.transform.position += platformMovement;
        }

        // 발판의 현재 위치를 저장하여 다음 프레임에서 비교
        lastPlatformPosition = platform.transform.position;
    }

    // 레이캐스트 박스로 캐릭터가 발판 위에 있는지 감지하는 함수
    private bool IsPlayerOnPlatform(GameObject platform)
    {
        Collider2D platformCollider = platform.GetComponent<Collider2D>();
        Vector2 boxCenter = new Vector2(platformCollider.bounds.center.x, platformCollider.bounds.max.y + 0.1f);
        Vector2 boxSize = new Vector2(platformCollider.bounds.size.x, 0.2f);

        // 레이캐스트 박스를 사용해 캐릭터가 발판 위에 있는지 감지
        RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Player"));
        return hit.collider != null;
    }


    // 충돌을 감지하고 부서지는 과정을 처리하는 코루틴
    private IEnumerator DetectPlatformCollision(GameObject platform, PlatformDataList.PlatformData platformData)
    {
        while (true)
        {
            Collider2D collider = platform.GetComponent<Collider2D>();
            Vector2 boxCenter = new Vector2(collider.bounds.center.x, collider.bounds.max.y + 0.1f);  // 충돌 감지를 위한 위치 상향
            Vector2 boxSize = new Vector2(collider.bounds.size.x, 0.2f);  // 충돌 영역을 약간 키움

            RaycastHit2D hit = Physics2D.BoxCast(boxCenter, boxSize, 0f, Vector2.down, 0.1f, LayerMask.GetMask("Player"));

            if (hit.collider != null)
            {
                StartCoroutine(ManageBreakablePlatform(platform, platformData));
                yield break;  // 부서지는 처리가 끝나면 이 코루틴 종료
            }

            yield return null;  // 다음 프레임까지 대기
        }
    }

    // 발판이 부서지고 재생성하는 과정을 관리하는 코루틴
    public IEnumerator ManageBreakablePlatform(GameObject platform, PlatformDataList.PlatformData platformData)
    {
        SpriteRenderer spriteRenderer = platform.GetComponent<SpriteRenderer>();

        BreakablePlatform breakablePlatform = platform.GetComponent<BreakablePlatform>();
        Sprite spriteAtTwoThirds = breakablePlatform.칠십퍼;
        Sprite spriteAtOneThird = breakablePlatform.삼십퍼;
        Sprite finalBrokenSprite = breakablePlatform.부서졌을때;
        Sprite originalSprite = breakablePlatform.백퍼;

        // 2/3 남았을 때 스프라이트 변경
        yield return new WaitForSeconds(platformData.delayBeforeBreak / 3f);
        spriteRenderer.sprite = spriteAtTwoThirds;

        // 1/3 남았을 때 스프라이트 변경
        yield return new WaitForSeconds(platformData.delayBeforeBreak / 3f);
        spriteRenderer.sprite = spriteAtOneThird;

        // 부서지기 직전 마지막 스프라이트
        yield return new WaitForSeconds(platformData.delayBeforeBreak / 3f);
        spriteRenderer.sprite = finalBrokenSprite;

        // 발판 비활성화
        platform.SetActive(false);

        // 리스폰 후 원래 상태로 복구
        yield return new WaitForSeconds(platformData.respawnDelay);
        platform.SetActive(true);
        spriteRenderer.sprite = originalSprite;  // 원래 상태로 복구

        // 발판이 다시 재생성되었으므로 다시 충돌 감지 코루틴을 시작
        StartCoroutine(DetectPlatformCollision(platform, platformData));
    }
}
