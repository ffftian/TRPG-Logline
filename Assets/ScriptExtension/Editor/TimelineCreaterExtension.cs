//#if UNITY_EDITOR
using Miao;
using Spine.Unity;
using Spine.Unity.Playables;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

/// <summary>
/// 这是一个演示，如何在trpg-log0-show插件中扩展自动创建timeline的方式，只在创建时使用。
/// </summary>
public static class TimelineCreaterExtension
{
    [TimeLineCreater(10,"摄像机看向角色")]
    public static void CameraFocusRole(string spineName, TimelineAsset timelineAsset)
    {
        if (string.IsNullOrEmpty(spineName)) return;

        if (timelineAsset.GetOutputTrack(5).hasClips)//有配音的情况下才创建
        {
            CameraFocusTrack focus = timelineAsset.CreateTrack<CameraFocusTrack>(nameof(CameraFocusRole));
            CameraFocusClip clip = (CameraFocusClip)(focus.CreateClip<CameraFocusClip>().asset);
        }
    }

    [TimeLineCreater(0,"给角色创建一条空Spine轨道")]
    public static void EmptyTrack(string spineName, TimelineAsset timelineAsset)
    {
        if (string.IsNullOrEmpty(spineName)) return;

        timelineAsset.CreateTrack<SpineAnimationStateTrack>("extension");
    }
}

//#endif