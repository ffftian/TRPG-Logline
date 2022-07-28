using Miao;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using System.Reflection;

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
        //#if !UNITY_EDITOR
        //serialPtr = 0;
        //#endif
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
                if (playable.GetGenericBinding(useTimeLineAsset.GetRootTrack(0)) == null)
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
                /////从8开始计算是否有额外对象
                //if (useTimeLineAsset.rootTrackCount >= 8)
                //{
                //    List<string> extension = QQLogSettings.LoadSettings().TimeLineExtension;
                //    for (int i = 0; i < extension.Count; i++)
                //    {
                //        //这里很难用反射实现自定义调用，先手写
                //        //MethodInfo[] methods = typeof(QQTimelineShowExtend).GetMethods();
                //        TrackAsset track = useTimeLineAsset.GetRootTrack(i + 7);
                //        if (extension[i] == track.name)
                //        {
                //            switch (extension[i])
                //            {
                //                case "CameraFocusRole":
                //                    playable.SetGenericBinding(useTimeLineAsset.GetRootTrack(i + 7), Camera.main);
                //                    var clips = useTimeLineAsset.GetRootTrack(i + 7).GetClips();
                //                    foreach (TimelineClip timelineClip in clips)
                //                    {
                //                        ExposedReference<Transform> asExposedReference = new ExposedReference<Transform>();
                //                        asExposedReference.defaultValue = roleGroup.Find(SerialData.roleName);
                //                        CameraFocusClip focusClip = (CameraFocusClip)timelineClip.asset;
                //                        focusClip.focus = asExposedReference;
                //                        ///默认拥有的类，没准以后会用到。
                //                        //ControlPlayableAsset controlPlayableAsset;
                //                    }
                //                    break;
                //            }
                //        }
                //    }
                //}
            }
            else
            {
                //Debug.Log("<color=blue>用户的Spine不存在</color>");
            }
            //文本
            if (useTimeLineAsset?.GetRootTrack(4) is DialogueControlTrack)
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
