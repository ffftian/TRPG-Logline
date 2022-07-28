using Spine.Unity;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QQLogSettings : ScriptableObject
{
    public const string settingPath = @"Assets\TRPGEditorSetting";
    public const string solderPath = "TRPGEditorSetting";
    public const string settingName = @"\TRPGEditorSetting.asset";
    public const string timeLineDirectory = "QQTimeLine";
    public QQMessageAsset QQMessageAsset;
    public Vector2Int messageListRange;
    public bool UseDefaultName;
    public string SpineAssetName;
    [Tooltip("生成时忽略需生成动画的用户名")]
    public string[] IgnoreName;
    public List<bool> extendMethodInfos = new List<bool>();

#if UNITY_EDITOR
    static QQLogSettings settings;
    public static QQLogSettings LoadSettings()
    {
        if (settings) return settings;

        settings = AssetDatabase.LoadAssetAtPath<QQLogSettings>(QQLogSettings.settingPath + QQLogSettings.settingName);
        if(settings == null)
        {
            settings =  QQLogSettings.CreateInstance<QQLogSettings>();
            AssetDatabase.CreateFolder("Assets", QQLogSettings.solderPath);
            AssetDatabase.CreateAsset(settings, QQLogSettings.settingPath + QQLogSettings.settingName);
        }
        return settings;
    }
#endif

}

