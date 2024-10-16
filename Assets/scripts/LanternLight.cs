using UnityEngine;


public class LanternLightController : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D spotLight;  // Spot Light 2D를 참조합니다.
    public float maxRange = 30f;  // 랜턴의 초기 최대 범위
    public float minRange = 2f;   // 랜턴의 최소 범위
    private float currentRange;

    private float drainRate;   // 빛이 줄어드는 속도

    void Start()
    {
        currentRange = maxRange;
        drainRate = (maxRange - minRange) / 20f;  // 랜턴 최대 범위에서 최소 범위까지 줄어드는데 걸리는 시간
        UpdateLanternLight();
    }

    void Update()
    {
        // 시간이 지남에 따라 빛의 범위가 줄어듭니다.
        currentRange -= drainRate * Time.deltaTime;
        currentRange = Mathf.Clamp(currentRange, minRange, maxRange);
        UpdateLanternLight();
    }

    public void RestoreLanternRange(float amount)
    {
        currentRange += amount;
        currentRange = Mathf.Clamp(currentRange, minRange, maxRange);
        UpdateLanternLight();
    }

    private void UpdateLanternLight()
    {
        // Spot Light 2D의 Range와 Intensity를 업데이트합니다.
        spotLight.pointLightOuterRadius = currentRange;  // Light 2D의 외부 반경 설정
        //spotLight.intensity = currentRange / maxRange;  // 범위에 비례해서 밝기도 조정합니다.
    }
}