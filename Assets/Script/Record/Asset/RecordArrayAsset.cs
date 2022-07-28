#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RecordArrayAsset : SerializedScriptableObject
{
    [ListDrawerSettings(ShowIndexLabels =true)]
    public string[] RecordArray;
    public static RecordArrayAsset GetAsset(string assetName, string ObjectName, int size)
    {
        RecordArrayAsset recordObjectAsset = AssetDatabase.LoadAssetAtPath<RecordArrayAsset>($"{QQLogSettings.settingPath}\\{assetName}-{ObjectName}.Asset");
        if (recordObjectAsset == null)
        {
            recordObjectAsset = new RecordArrayAsset();
            recordObjectAsset.RecordArray = new string[size];
            AssetDatabase.CreateAsset(recordObjectAsset, $"{QQLogSettings.settingPath}\\{assetName}-{ObjectName}.Asset");
        }
        return recordObjectAsset;
    }
    /// <summary>
    /// 超快
    /// </summary>
    /// <param name="index"></param>
    /// <param name="keyValueJson"></param>
    public void RecordValue(int index, string keyValueJson)
    {
        RecordArray[index] = keyValueJson;
        EditorUtility.SetDirty(this);
    }
    public string RecoverValue(int index)
    {
        return RecordArray[index];
    }
    public string LastRecoverValue(int index)
    {
        for (int i = index - 1; i >= 0; i--)
        {
            if (!string.IsNullOrEmpty(RecordArray[i]))
            {
                return RecordArray[i];
            }
        }
        return "";
    }

}
#endif