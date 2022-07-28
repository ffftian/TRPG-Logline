#if UNITY_EDITOR
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(RecordManager))]
public abstract class BaseAnmationRecord : MonoBehaviour, IRecover
{
    public Transform recordObjectTransform;
    RecordManager recordManager;
    public RecordAnmationAsset RecordAnmationAsset(string assetName)
    {
        return global::RecordAnmationAsset.GetAsset(assetName, recordObjectTransform.name);
    }


    /// <summary>
    /// 因为要数据化编辑器使用，所以要写这里
    /// </summary>
    virtual protected void OnValidate()
    {
        recordManager = this.GetComponent<RecordManager>();
        recordManager.RecordAnimationTrackDataCall -= RecordData;
        recordManager.RecordAnimationTrackDataCall += RecordData;
    }
    public struct TimeLineKeyFrame
    {
        public float LineTime;
        public float Value; 
    }


    /// <summary>
    /// 记录动画
    /// </summary>
    /// <param name="messageAssetName"></param>
    /// <param name="targetName"></param>
    /// <param name="serial"></param>
    /// <param name="animationClip"></param>
    /// <param name="editorCurveBindings"></param>
   // abstract protected void RecordData(string messageAssetName, string targetName, int serial, AnimationClip animationClip, EditorCurveBinding[] editorCurveBindings);
    virtual protected void RecordData(string messageAssetName, string targetName, int serial, AnimationTrack animationTrack)
    {
        if (recordObjectTransform==null || targetName != recordObjectTransform.name) return;
        Dictionary<string, TimeLineKeyFrame> saveKeyframeDict = new Dictionary<string, TimeLineKeyFrame>();//最终要存的
        RegisterAnimationTrack(ref saveKeyframeDict, animationTrack);
        foreach (AnimationTrack childAnimationTrack in animationTrack.GetChildTracks())//存子轨道数据
        {
            
            RegisterAnimationTrack(ref saveKeyframeDict, childAnimationTrack);
        }
        JObject curveValue = new JObject();
        foreach (var keyFrame in saveKeyframeDict)
        {
            curveValue.Add(keyFrame.Key, keyFrame.Value.Value);
        }
        RecordAnmationAsset(messageAssetName).RecordValue(serial, curveValue.ToString());



    }
    void RegisterAnimationTrack(ref Dictionary<string, TimeLineKeyFrame> saveKeyframe, AnimationTrack animationTrack)
    {
        AnimationClip infiniteClip = animationTrack.infiniteClip;
        if (infiniteClip != null)//处理特供无限轨道
        {
            //animationTrack.clip
            CashKeyframe(ref saveKeyframe, infiniteClip, (float)animationTrack.start);
        }
        else//非无限轨道的情况下，寻找默认Clip
        {
            foreach (TimelineClip clip in animationTrack.GetClips())
            {
               //AnimationTrack animationTrack1 = clip as AnimationTrack;
                CashKeyframe(ref saveKeyframe, clip.animationClip, (float)clip.start);
            }
        }
    }



    void CashKeyframe(ref Dictionary<string, TimeLineKeyFrame> saveKeyFrame, AnimationClip animationClip,float lineTime)
    {

        EditorCurveBinding[] editorCurveBindings = AnimationUtility.GetCurveBindings(animationClip);
        for (int i = 0; i < editorCurveBindings.Length; i++)
        {
            EditorCurveBinding curve = editorCurveBindings[i];
            AnimationCurve curves = AnimationUtility.GetEditorCurve(animationClip, curve);

            TimeLineKeyFrame timeLineKeyFrame = new TimeLineKeyFrame();
            timeLineKeyFrame.LineTime = curves.keys[curves.length - 1].time + lineTime;
            if (saveKeyFrame.ContainsKey(curve.propertyName))
            {
                if (saveKeyFrame[curve.propertyName].LineTime < timeLineKeyFrame.LineTime)
                {
                    timeLineKeyFrame.Value = curves.keys[curves.length - 1].value;
                    saveKeyFrame[curve.propertyName] = timeLineKeyFrame;
                }
            }
            else
            {
                timeLineKeyFrame.Value = curves.keys[curves.length - 1].value;
                saveKeyFrame[curve.propertyName] = timeLineKeyFrame;
            }
        }
    }


    /// <summary>
    /// 恢复存储的值,没有的情况则取默认值
    /// </summary>
    /// <param name="currentIndex"></param>
    abstract public void RecoverData(string messageAssetName, int currentIndex);

    abstract protected void RecordDefultValue();
}
#endif
