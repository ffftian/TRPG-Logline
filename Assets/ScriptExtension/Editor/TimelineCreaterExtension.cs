//#if UNITY_EDITOR
using Miao;
using Spine.Unity;
using Spine.Unity.Playables;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

/// <summary>
/// 这是一个演示，如何在trpg-logline插件中扩展自动创建timeline的方式，只在创建时使用。
/// </summary>
public static class TimelineCreaterExtension
{
    [TimeLineCreater(10, "摄像机看向角色")]
    public static void CameraFocusRole(string spineName, TimelineAsset timelineAsset, MessageData messageData)
    {
        if (string.IsNullOrEmpty(spineName)) return;

        if (timelineAsset.GetOutputTrack(5).hasClips)//有配音的情况下才创建
        {
            CameraFocusTrack focus = timelineAsset.CreateTrack<CameraFocusTrack>(nameof(CameraFocusRole));
            CameraFocusClip clip = (CameraFocusClip)(focus.CreateClip<CameraFocusClip>().asset);
        }
    }

    [TimeLineCreater(0, "给角色创建一条空Spine轨道")]
    public static void EmptyTrack(string spineName, TimelineAsset timelineAsset, MessageData messageData)
    {
        if (string.IsNullOrEmpty(spineName)) return;

        timelineAsset.CreateTrack<SpineAnimationStateTrack>("extension");
    }
    [TimeLineCreater(1, "浮现于角色所说对话")]
    public static void RoleDialogue(string spineName, TimelineAsset timelineAsset, MessageData messageData)
    {
        //提取出对话  
        var dialouges = messageData.log.Split(new string[] { "\"", "“", "”" }, StringSplitOptions.None);

        if (dialouges.Length > 3)
        {
            DialogueControlTrack dialogueControlTrack = timelineAsset.CreateTrack<DialogueControlTrack>(nameof(spineName));
            float clipDuration = 0;
            ///添加对话生成,标准为(描述"对话"描述"对话"描述)这样间隔
            for (int i = 0; i < dialouges.Length - 1; i += 2)
            {


                TimelineClip timelineClip = dialogueControlTrack.CreateClip<DialogueControlClip>();
                DialogueControlClip dialougeClip = (DialogueControlClip)(timelineClip.asset);//创建一个节拍
                clipDuration += dialouges[i].Length * 0.12f;
                timelineClip.start = clipDuration;

                dialougeClip.template.dialogue = dialouges[i + 1];
                clipDuration += dialouges[i + 1].Length * 0.12f;
                timelineClip.duration = dialouges[i + 1].Length * 0.12f;
            }
        }
    }
}

//#endif