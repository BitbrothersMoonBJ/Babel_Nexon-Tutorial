using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemAPrefab;
    public GameObject itemBPrefab;
    public GameObject itemCPrefab;

    public Tilemap[] tilemaps;  // 여러 개의 타일맵을 참조
    public float spawnInterval = 3f;
    private float nextSpawnTime;

    public float yOffset = 0.2f;  // 바닥 위에 생성될 오프셋

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnItem();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnItem()
    {
        // 랜덤하게 타일맵 선택
        Tilemap selectedTilemap = tilemaps[Random.Range(0, tilemaps.Length)];

        // 선택된 타일맵의 랜덤한 타일 위치 선택
        Vector3Int randomCellPosition = GetRandomTilePosition(selectedTilemap);
        Vector3 spawnPosition = selectedTilemap.CellToWorld(randomCellPosition) + selectedTilemap.cellSize / 2;

        // Y축 오프셋 추가
        spawnPosition.y += yOffset;

        // 랜덤한 아이템 선택
        float randomValue = Random.Range(0f, 1f);
        GameObject itemToSpawn;

        if (randomValue < 0.5f)  // 50% 확률로 A 아이템
        {
            itemToSpawn = itemAPrefab;
        }
        else if (randomValue < 0.8f)  // 30% 확률로 B 아이템
        {
            itemToSpawn = itemBPrefab;
        }
        else  // 20% 확률로 C 아이템
        {
            itemToSpawn = itemCPrefab;
        }

        // 아이템 생성
        Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);
    }

    Vector3Int GetRandomTilePosition(Tilemap tilemap)
    {
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int randomPosition;
        int maxAttempts = 100;
        int attempt = 0;

        do
        {
            randomPosition = new Vector3Int(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax),
                bounds.z);
            attempt++;
        } while (!tilemap.HasTile(randomPosition) && attempt < maxAttempts);

        return randomPosition;
    }
}