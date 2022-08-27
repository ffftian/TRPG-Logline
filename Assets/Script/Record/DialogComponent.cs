
using Miao;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using System.Reflection;
using Spine.Unity.Playables;

/// <summary>
/// 对话控制器脚本
/// </summary>
[DisallowMultipleComponent]
public class DialogComponent : MonoBehaviour
{
    public PlayableDirector playable;
    public TyperDialogue dialogue;
    public int serialPtr;
    public Transform roleGroup;
    protected Dictionary<string, SkeletonAnimation> roles = new Dictionary<string, SkeletonAnimation>();
    private string lastID = string.Empty;
    public Text NameText;
    [Sirenix.Serialization.OdinSerialize]
    [Tooltip("角色名称与对应颜色")]
    public ShowComponentSetting showComponentSetting;

    public QQMessageAsset messageAsset;
    public TimelineAsset useTimeLineAsset;
#if UNITY_EDITOR
    public event Action<TimelineAsset> OnTimeLineLeave;
    /// <summary>
    /// 如果这个报错，检查一下是否是误点了什么TimeLineAsset，重新将UseTimeLineAsset设空
    /// </summary>
    public void OnTimeLineLeaveInvoke()
    {
        if (useTimeLineAsset == null) return;
        OnTimeLineLeave?.Invoke(useTimeLineAsset);
    }
#endif

    public void OnValidate()
    {
        if (roleGroup != null)
        {
            roles.Clear();
            for (int i = 0; i < roleGroup.childCount; i++)
            {
                Transform roleChild = roleGroup.GetChild(i);
                roles.Add(roleChild.name, roleChild.GetComponent<SkeletonAnimation>());
            }
        }
    }

    public void Start()
    {
#if !UNITY_EDITOR
        serialPtr = 0;
#endif
        SetNameText(messageAsset.messageDataList[serialPtr].roleName);
        MessageRuning();
    }
    public void SetNameText(string name)
    {
        Color nameColor;
        showComponentSetting.NameColors.TryGetValue(name, out nameColor);
        NameText.text = name;
        NameText.color = nameColor == Color.clear ? Color.white : nameColor;
    }



    /// <summary>
    /// 运行游戏后，从当前Ptr位置持续向前播放。
    /// </summary>
    public void MessageRuning()
    {
        playable.Play();
        playable.extrapolationMode = DirectorWrapMode.None;
        playable.stopped += Next;
    }

    private void Next(PlayableDirector obj)
    {
        serialPtr++;
        if (serialPtr == messageAsset.messageDataList.Count)
        {
            Debug.Log("<color=green>播放完毕</color> ");
            return;
        }
        SelectMessage(serialPtr);
        obj.Play();
    }

    public void SelectMessage(int Serial)
    {
        MessageData serialData = messageAsset.messageDataList[Serial];
        SetNameText(serialData.roleName);
        //从指定位置获取，绑定对应序列的TimelineAsset。
        string timeLinePath = $@"{QQLogSettings.timeLineDirectory}\{messageAsset.name}\{serialData.fileID}";
        useTimeLineAsset = Resources.Load<TimelineAsset>(timeLinePath);
        playable.playableAsset = useTimeLineAsset;
#if UNITY_EDITOR
        if (playable.playableAsset == null) return;
        if (lastID != serialData.fileID)
        {
            lastID = serialData.fileID;
            roles.TryGetValue(serialData.roleName,out var skeletonAnimation);
            SkeletonAnimation PlayRoleAnimation = skeletonAnimation;
            if (PlayRoleAnimation != null)
            {
                //Debug.Log($"切换躯体{PlayRoleAnimation.name}");
                //躯干，表情，嘴型,特殊
                if (useTimeLineAsset.GetRootTrack(0)is SpineAnimationStateTrack && playable.GetGenericBinding(useTimeLineAsset.GetRootTrack(0)) == null)
                {
                    playable.SetGenericBinding(useTimeLineAsset.GetRootTrack(0), PlayRoleAnimation);
                    playable.SetGenericBinding(useTimeLineAsset.GetRootTrack(1), PlayRoleAnimation);
                    playable.SetGenericBinding(useTimeLineAsset.GetRootTrack(2), PlayRoleAnimation);
                    playable.SetGenericBinding(useTimeLineAsset.GetRootTrack(3), PlayRoleAnimation);
                }
                ///处理额外轨道赋值对象
                foreach(TrackAsset track in useTimeLineAsset.GetRootTracks())
                {
                    TimelineSelectManager.TimelineSelectMethodCall(this, serialData, playable, track);
                }
            }
            //文本的两种初始赋值处理，对应旁白预设轨道和角色预设轨道
            if (useTimeLineAsset?.GetRootTrack(0) is DialogueControlTrack)
            {
                playable.SetGenericBinding(useTimeLineAsset.GetRootTrack(0), dialogue);
            }
            else if (useTimeLineAsset?.GetRootTrack(4) is DialogueControlTrack)
            {
                playable.SetGenericBinding(useTimeLineAsset.GetRootTrack(4), dialogue);
            }
            #region 另一种写法
            //foreach (var Line in playable.playableAsset.outputs)
            //{
            //    ///播放时绑定到特定角色的Animation
            //    var trackName = Line.streamName;
            //    if (trackName.Contains("Spine"))
            //    {
            //        playable.SetGenericBinding(Line.sourceObject, PlayRoleAnimation);
            //    }
            //    if(trackName.Equals("Dialogue"))
            //    {
            //        playable.SetGenericBinding(Line.sourceObject, dialogue);
            //    }
            //}
            #endregion
        }
#endif
    }
    public void PlayMessage()
    {
        playable.Play();
    }
}
