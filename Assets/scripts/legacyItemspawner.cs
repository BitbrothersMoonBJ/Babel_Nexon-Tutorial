using UnityEngine;

public class legacyItemSpawner : MonoBehaviour
{
    public GameObject itemAPrefab;
    public GameObject itemBPrefab;
    public GameObject itemCPrefab;

    public float spawnInterval = 3f;
    private float nextSpawnTime;

    void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && GameObject.FindGameObjectsWithTag("Item").Length == 0)
        {
            SpawnItem();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnItem()
    {
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

        // X 위치를 -2.75에서 2.75 사이로 제한
        float randomX = Random.Range(-2.75f, 2.75f);
        Vector3 spawnPosition = new Vector3(randomX, 6f, 0f);  // Y 값을 6으로 고정

        Instantiate(itemToSpawn, spawnPosition, Quaternion.identity);
    }
}
