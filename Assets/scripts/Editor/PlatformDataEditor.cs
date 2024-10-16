/*잘 안됨 사용 안 함
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PlatformDataList))]
public class PlatformDataListEditor : Editor
{
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        // ReorderableList 생성
        reorderableList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("platformDatas"),  // SerializedProperty로 List를 찾음
            true, true, true, true);

        // 리스트 헤더
        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Platform Data List");
        };

        // 각 요소 그리기
        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            // platformPrefab 필드
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, rect.width - 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("platformPrefab"), new GUIContent("Prefab"));

            // startPoint 필드
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 20, rect.width - 60, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("startPoint"), new GUIContent("Start Point"));

            // 여유 공간 추가
            rect.y += 40;

            // moving 필드
            SerializedProperty movingProp = element.FindPropertyRelative("moving");
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 40, rect.width - 60, EditorGUIUtility.singleLineHeight),
                movingProp, new GUIContent("Moving"));

            // breakable 필드
            SerializedProperty breakableProp = element.FindPropertyRelative("breakable");
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y + 60, rect.width - 60, EditorGUIUtility.singleLineHeight),
                breakableProp, new GUIContent("Breakable"));
        };

        // 항목 추가 시 기본 값 설정
        reorderableList.onAddCallback = (ReorderableList list) =>
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("platformPrefab").objectReferenceValue = null;
            element.FindPropertyRelative("startPoint").vector3Value = Vector3.zero;
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
*/