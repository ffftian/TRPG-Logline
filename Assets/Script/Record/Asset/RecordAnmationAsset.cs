#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

[CreateAssetMenu]
public class RecordAnmationAsset : SerializedScriptableObject//奥丁型的高阶资源存储
{
    //[OdinSerializable]
    public Dictionary<int, string> RecordDict;

    public static RecordAnmationAsset GetAsset(string assetName, string ObjectName)
    {
        RecordAnmationAsset recordObjectAsset = AssetDatabase.LoadAssetAtPath<RecordAnmationAsset>($"{QQLogSettings.settingPath}\\{assetName}-{ObjectName}.Asset");
        if (recordObjectAsset == null)
        {
            recordObjectAsset = new RecordAnmationAsset();
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
            RecordDict = RecordDict.OrderBy(x => x.Key).ToDictionary(c=>c.Key,c=>c.Value);
        }
        EditorUtility.SetDirty(this);
    }
    /// <summary>
    /// 因为动画做的都是相对偏移记录，所以需要遍历每一项字典中存在的值，需要从前直到后。
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public List<JObject> GetkeyValueJsonFrontToBack(int index)
    {
        //string value;
        List<JObject> JList = new List<JObject>();

        //这个循环是先入先循环
        foreach (var keyValue in RecordDict)
        {
            if (keyValue.Key >= index)
            {
                //如果key大于当前输入的序号，则跳出循环。
                break;
              
                //continue;
            }
            JList.Add(JsonConvert.DeserializeObject<JObject>(keyValue.Value));

        }
        return JList;
    }

}
#endif

