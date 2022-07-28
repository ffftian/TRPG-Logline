using System.IO;
using UnityEditor;
using UnityEngine;
//[CreateAssetMenu(menuName = "TimelineCreateSettings", fileName =  "TimelineCreateSettings")]
public class TimelineCreaterSettings : ScriptableObject
{
    public const string settingName = @"\TimelineCreaterSettings.asset";

    [Header("这是导出时口型匹配的设置（高级）")]
    [Tooltip("最大口型阈值")]
    public float MaxThreshold = 0.06f;
    [Tooltip("最小口型阈值")]
    public float MinThreshold = 0.01f;
    [Tooltip("多少时间内的截面视为一个音频嘴型匹配切片")]
    public float SectionLength = 0.02f;
    [Tooltip("多少个截面后，才视为可更改嘴型(避免过快嘴型交接)")]
    public int ChangeMouthThreshold = 4;

#if UNITY_EDITOR
    static TimelineCreaterSettings settings;
    public static TimelineCreaterSettings LoadSettings()
    {
        if (settings) return settings;

        settings = AssetDatabase.LoadAssetAtPath<TimelineCreaterSettings>(QQLogSettings.settingPath + settingName);
        if (settings == null)
        {
            settings = QQLogSettings.CreateInstance<TimelineCreaterSettings>();
            //if (!AssetDatabase.IsValidFolder(Path.Combine("Assets", QQLogSettings.solderPath)))
            //{
            //    AssetDatabase.CreateFolder("Assets", QQLogSettings.solderPath);
            //}
            AssetDatabase.CreateAsset(settings, QQLogSettings.settingPath + settingName);
        }
        return settings;
    }
#endif
}