#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(DialogComponent))]
public class RecordManager : MonoBehaviour
{

    DialogComponent dialogComponent;
    public bool EnableRecord;
    virtual protected void OnValidate()
    {
        dialogComponent = this.GetComponent<DialogComponent>();
        dialogComponent.OnTimeLineLeave -= Record;
        dialogComponent.OnTimeLineLeave += Record;
    }
    #region 记录存储
    public event Action<string,string,int, AnimationTrack> RecordAnimationTrackDataCall;
    public event Action<string,string, int,object> RecordDataCall;
    /// <summary>
    /// 如果离开当前TimeLine，就进行记录
    /// 当有子轨道的情况下优先记录子轨道进行最后的记录覆盖
    /// </summary>
    /// <param name="timelineAsset"></param>
    public void Record(TimelineAsset timelineAsset)
    {
        if (!EnableRecord) return;

        foreach (TrackAsset track in timelineAsset.GetOutputTracks())
        {
            if (track is AnimationTrack)//一整个轨道池
            {
                
                RecordAnimationTrackDataCall(dialogComponent.messageAsset.name, dialogComponent.playable.GetGenericBinding(track).name, dialogComponent.serialPtr, track as AnimationTrack);
                //string targetName = dialogComponent.playable.GetGenericBinding(animationtTrack).name;
            }
            if(track is IRecordTack)
            {
                //直接用会说是失效的dialogComponent.playable.playableGraph
                //object data = (track as IRecordTack).ValueSave(dialogComponent.playable.playableGraph);
                object data = (track as IRecordTack).SaveValue;
                if (data != default(object))
                {
                    TrackRecord(dialogComponent.playable.GetGenericBinding(track).name, data);
                }
            }
        }
    }
    public void AnimationTrackClipAnalysisRecord(string targetName, AnimationClip animationClip)
    {
        EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(animationClip);
      
    }
    public void TrackRecord(string targetName,object value)
    {
        RecordDataCall?.Invoke(dialogComponent.messageAsset.name,targetName, dialogComponent.serialPtr, value);
    }

    #endregion

    /// <summary>
    /// 还原存储的数据
    /// </summary>
    [Button("同步最近的动画位置坐标")]
    public void Recover()
    {
        foreach (IRecover baseRecord in GetComponents<IRecover>())
        {
            baseRecord.RecoverData(dialogComponent.messageAsset.name, dialogComponent.serialPtr);
        }
        //RecoverData();
    }
    //public void RecoverData()
    //{
    //    foreach (IRecover baseRecord in GetComponents<IRecover>())
    //    {
    //        baseRecord.RecoverData(dialogComponent.messageAsset.name, dialogComponent.serialPtr);
    //    }
    //}

}
#endif

