using UnityEngine;
using UnityEngine.UI;

public class JumpGauge : MonoBehaviour
{
    [SerializeField] private Jumping jumpingScript; // Jumping 스크립트 참조
    [SerializeField] private Slider jumpSlider; // UI 슬라이더
    [SerializeField] private Image fillImage; // 게이지 채울 이미지

    void Start()
    {
        // 슬라이더의 최소값과 최대값 설정
        jumpSlider.minValue = 0f; // 게이지가 항상 0에서부터 차오르도록 설정
        jumpSlider.maxValue = 1f; // 슬라이더의 전체 범위는 1로 설정
        jumpSlider.value = 1f; // 슬라이더는 항상 가득 찬 상태 유지

        // Fill 이미지의 초기 상태를 0으로 설정 (비어 있는 상태로 시작)
        fillImage.fillAmount = 0f;

        // 처음에 슬라이더를 비활성화 (스페이스바를 누를 때만 나타남)
        jumpSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (jumpingScript != null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                jumpSlider.gameObject.SetActive(true); // 슬라이더 활성화

                // currentHorizontalForce를 0과 1 사이로 변환하여 Fill만 업데이트
                float normalizedValue = Mathf.InverseLerp(jumpingScript.MinHorizontaljumpForce, jumpingScript.MaxHorizontaljumpForce, jumpingScript.currentHorizontalForce);
                fillImage.fillAmount = normalizedValue; // Fill 이미지의 채워진 정도를 업데이트
            }
            else
            {
                jumpSlider.gameObject.SetActive(false); // 슬라이더 비활성화
            }
        }
    }
}
