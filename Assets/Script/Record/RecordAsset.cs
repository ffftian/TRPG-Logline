using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

//[Obsolete("一劳永逸的存储非常繁琐，应该将其分化为基于单个角色的存储")]
//[CreateAssetMenu]
//public class PlayRecordGroupAsset : ScriptableObject
//{
//    private static PlayRecordGroupAsset playRecordGroupAsset;
//    public HashSet<string> targetPool = new HashSet<string>();//有过变更对象记录
//    public Dictionary<string, PlayRecordAsset> playRecordAssetDict = new Dictionary<string, PlayRecordAsset>();
//    /// <summary>
//    /// 
//    /// </summary>
//    /// <param name="targetObject=隶属于哪个目标对象的动画值，如角色，摄像机"></param>
//    /// <param name="serial=隶属于的动画时间线"></param>
//    /// <param name="parameterName"></param>
//    /// <param name="value"></param>
//    public void PushRecordAsset(string targetObject, int serial, string parameterName, object value)
//    {
//        PlayRecordAsset playRecordAsset;
//        playRecordAssetDict.TryGetValue(targetObject,out playRecordAsset);
//         if(playRecordAsset==null)
//        {
//            playRecordAsset = new PlayRecordAsset();
//        }
//        if(playRecordAsset.RecordAsset==null)
//        {
//            playRecordAsset.RecordAsset = new Dictionary<int, Dictionary<string, object>>();
//        }



//        if (playRecordAsset.RecordAsset[serial][parameterName] == null)
//        {
//            playRecordAssetDict[targetObject].RecordAsset[serial].Plus(parameterName, value);
//        }
//        else
//        {
//            playRecordAssetDict[targetObject].RecordAsset[serial][parameterName] = value;
//        }

//        EditorUtility.SetDirty(this);
//    }


//    public static PlayRecordGroupAsset InstanceAsset()
//    {
//        if (playRecordGroupAsset == null)
//        {
//            playRecordGroupAsset = AssetDatabase.LoadAssetAtPath<PlayRecordGroupAsset>($"{QQLogSettings.settingPath}\\PlayRecordGroupAsset.Asset");
//            if (playRecordGroupAsset == null)
//            {
//                playRecordGroupAsset = new PlayRecordGroupAsset();
//                AssetDatabase.CreateAsset(playRecordGroupAsset, $"{QQLogSettings.settingPath}\\PlayRecordGroupAsset.Asset");
//            }
//        }
//        return playRecordGroupAsset;
//    }
//}
///// <summary>
///// 这里写个取上搜索法，寻找场景中所有需移动对象被移动的上级对象。
///// 所以这里先只处理角色，摄像机记录另算。
///// </summary>
//[Serializable]
//public class PlayRecordAsset
//{
//    public Dictionary<int, Dictionary<string, object>> RecordAsset;

//    //Vector2 Postion;
//}
///// <summary>
///// 旋转平移缩放（局部）
///// </summary>
////public struct RTS
////{
////    float angle;
////    Vector3 Position;
////    Vector3 Scale;
////}

