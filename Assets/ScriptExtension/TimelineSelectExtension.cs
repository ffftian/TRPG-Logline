using Miao;
using Spine.Unity;
using Spine.Unity.Playables;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// ����һ����ʾ��������Dialog�ű�ѡ���ض����ʱ��Ԥ�ȸ�ֵ
/// </summary>
public static class TimelineSelectExtension
{
    [TimelineSelectAttribute]
    public static void CameraFocusRole(DialogComponent dialogComponent, MessageData serialData, PlayableDirector playable, TrackAsset track)
    {
        CameraFocusTrack cameraFocusTrack = track as CameraFocusTrack;
        if(cameraFocusTrack != null)
        {
            playable.SetGenericBinding(track, Camera.main);
            //var clips = cameraFocusTrack.GetClips();
            //foreach (TimelineClip timelineClip in clips)
            //{
            //    ExposedReference<Transform> asExposedReference = new ExposedReference<Transform>();
            //    asExposedReference.defaultValue = dialogComponent.roleGroup.Find(serialData.roleName);
            //    CameraFocusClip focusClip = (CameraFocusClip)timelineClip.asset;
            //    focusClip.focus = asExposedReference;
            //    ///Ĭ��ӵ�е��࣬û׼�Ժ���õ���
            //    //ControlPlayableAsset controlPlayableAsset;
            //}
        }
    }

}
