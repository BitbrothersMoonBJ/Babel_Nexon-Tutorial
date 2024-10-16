using UnityEngine;


public class LanternLight2D : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D freeformLight2D; // Freeform Light 2D 컴포넌트
    public float maxLightRadius = 10f; // 최대 빛의 범위
    public float shrinkRate = 0.5f; // 빛이 줄어드는 속도
    public float minRadius = 0.1f; // 최소 빛 범위

    void Start()
    {
        // 시작할 때 빛의 최대 범위를 설정
        freeformLight2D.shapeLightFalloffSize = maxLightRadius;
    }

    void Update()
    {
        // 시간이 지남에 따라 빛의 범위를 줄임
        if (freeformLight2D.shapeLightFalloffSize > minRadius)
        {
            freeformLight2D.shapeLightFalloffSize -= shrinkRate * Time.deltaTime;
        }
        else
        {
            // 최소 범위를 넘지 않도록 제한
            freeformLight2D.shapeLightFalloffSize = minRadius;
        }
    }
}
