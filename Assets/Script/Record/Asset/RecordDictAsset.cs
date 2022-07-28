#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public class RecordDictAsset : SerializedScriptableObject
{

    public Dictionary<int, string> RecordDict;

    public static RecordDictAsset GetAsset(string assetName, string ObjectName)
    {
        RecordDictAsset recordObjectAsset = AssetDatabase.LoadAssetAtPath<RecordDictAsset>($"{QQLogSettings.settingPath}\\{assetName}-{ObjectName}.Asset");
        if (recordObjectAsset == null)
        {
            recordObjectAsset = new RecordDictAsset();
            recordObjectAsset.RecordDict = new Dictionary<int, string>();
            AssetDatabase.CreateAsset(recordObjectAsset, $"{QQLogSettings.settingPath}\\{assetName}-{ObjectName}.Asset");
        }
        return recordObjectAsset;
    }
    public void RecordValue(int index, string keyValueJson)
    {
        if (RecordDict.ContainsKey(index))
        {
            RecordDict[index] = keyValueJson;
        }
        else
        {
            RecordDict.Add(index, keyValueJson);
            //因为有可能出现后来新增Anmation timeline更改前面的数据，但保存后字典会添在末尾，会出问题，固每次记录需要重排鉴定。
            RecordDict = RecordDict.OrderBy(x => x.Key).ToDictionary(c => c.Key, c => c.Value);
        }

        EditorUtility.SetDirty(this);
    }
}
#endif