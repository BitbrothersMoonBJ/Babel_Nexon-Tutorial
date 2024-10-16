using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;

    void Start()
    {
        // 게임 시작 시 옵션 메뉴는 비활성화되어 있어야 합니다.
        optionsPanel.SetActive(false);
    }

    void Update()
    {
        // ESC 키를 누르면 옵션 메뉴를 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptions();
        }
    }

    public void ToggleOptions()
    {
        // 옵션 패널 활성화/비활성화 토글
        optionsPanel.SetActive(!optionsPanel.activeSelf);

        // 게임을 일시 정지 또는 재개
        if (optionsPanel.activeSelf)
        {
            Time.timeScale = 0f; // 게임 일시 정지
        }
        else
        {
            Time.timeScale = 1f; // 게임 재개
        }
    }

    public void RestartGame()
    {
        // 현재 씬을 다시 로드 (처음부터 하기)
        Time.timeScale = 1f; // 게임 재개
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        // 게임 종료
        Application.Quit();
    }
}