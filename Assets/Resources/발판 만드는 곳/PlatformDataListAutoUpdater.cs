using UnityEngine;
using UnityEditor;
using System.IO;

public class PlatformDataListAutoUpdater : AssetPostprocessor
{
    // 코드가 변경되면 호출됨
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets)
        {
            // 특정 스크립트가 변경되면 해당 데이터 리스트 갱신
            if (asset.EndsWith("PlatformDataList.cs"))
            {
                UpdateDataList(asset);
            }
        }
    }

    // 데이터 리스트 갱신 로직
    static void UpdateDataList(string assetName)
    {
        if (assetName.Contains("PlatformDataList"))
        {
            Debug.Log("PlatformDataList has been updated.");
            // 여기에 PlatformDataList 관련 로직 추가
        }
    }
}
