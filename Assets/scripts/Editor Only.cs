using UnityEngine;

public class DisableEditorOnlyLight : MonoBehaviour
{
    void Start()
    {
        if (Application.isPlaying)
        {
            // EditorOnly 태그가 붙은 모든 오브젝트를 찾아 비활성화합니다.
            GameObject[] editorOnlyObjects = GameObject.FindGameObjectsWithTag("EditorOnly");

            foreach (GameObject obj in editorOnlyObjects)
            {
                obj.SetActive(false);
            }
        }
    }
}
