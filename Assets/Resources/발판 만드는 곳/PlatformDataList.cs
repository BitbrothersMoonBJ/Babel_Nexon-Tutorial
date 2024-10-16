using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlatformDataList", menuName = "Platform/PlatformDataList", order = 1)]
public class PlatformDataList : ScriptableObject
{
    public PlatformData[] platformDatas;

    [System.Serializable]
    public class PlatformData
    {
        public GameObject platformPrefab;
        public Vector3 startPoint;

        public bool moving;
        public Vector2 movementOffset;
        public float speed;
        public bool moveOnPlayerContact;

        public bool breakable;
        public float delayBeforeBreak;   // 부서지기 전 대기 시간
        public float respawnDelay;       // 재생성 대기 시간
    }
}
