using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Miao
{
    [TrackColor(0.5f, 0.5f, 0.5f)]
    [TrackBindingType(typeof(Camera))]
    [TrackClipType(typeof(CameraFocusClip))]
   // [TrackClipType(typeof(ColorSpineClip))]
    public class CameraFocusTrack : TrackAsset, IRecordTack//需记录轨道组
    {
        PlayableGraph playableGraph;

        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            this.playableGraph = graph;
            //Debug.Log($"现在赋值了{playableGraph}");
            return base.CreatePlayable(graph, gameObject, clip);
        }
        /// <summary>
        /// 这个会有个小问题，镜头和角色同步移动的情况下，可能会保存位置出问题
        /// </summary>
        public object SaveValue
        {
            get
            {
                var startPos = Camera.main.transform.position;
                var clips = (this.GetClips() as TimelineClip[]);
                if (clips.Length == 0) return default;
                if (!playableGraph.IsValid())
                {
                    Debug.LogError(playableGraph + "为空");
                    return default;
                }
                var tailDataClip = ((clips[clips.Length - 1]));
                var endCameraFocusClip = ((CameraFocusClip)(tailDataClip.asset));

                Transform target = endCameraFocusClip.focus.Resolve(playableGraph.GetResolver());
                if (target == null) return Vector3.zero;//没有保存就返回zero
                var endPos = target.position;
                if (endCameraFocusClip.template.LockX)
                {
                    endPos.x = startPos.x;
                }
                if (endCameraFocusClip.template.LockY)
                {
                    endPos.y = startPos.y;
                }
                endPos.z = startPos.z;
                return endPos;//尾部的坐标系
            }
        }
        #region 旧的

        //public object SaveValue
        //{
        //    get
        //    {
        //        var clips = (this.GetClips() as TimelineClip[]);
        //        if (clips.Length == 0) return default;
        //        if (!playableGraph.IsValid())
        //        {
        //            Debug.Log(playableGraph + "为空");
        //            return default;
        //        }

        //        var startPos = Camera.main.transform.position;//可能是因为已经将轨道销毁的原因所以拿不到

        //        var tailDataClip = ((clips[clips.Length - 1]));
        //        var endCameraFocusClip = ((CameraFocusClip)(tailDataClip.asset));

        //        Transform target = endCameraFocusClip.focus.Resolve(playableGraph.GetResolver());
        //        var endPos = target.position;

        //        Vector3 offsetPos = Vector3.zero;
        //        Debug.Log("摄像机的原始值为" + startPos);
        //        Debug.Log("目标位置的原始值为" + endPos);
        //        if (!endCameraFocusClip.template.LockX)
        //        {
        //            offsetPos.x = startPos.x - endPos.x;
        //        }
        //        if (!endCameraFocusClip.template.LockY)
        //        {
        //            offsetPos.y = startPos.y - endPos.y;
        //        }
        //        return offsetPos;//尾部的坐标系
        //    }
        //}

        //public object SaveValue
        //{
        //    get
        //    {
        //        var clips = (this.GetClips() as TimelineClip[]);
        //        if (clips.Length == 0) return default;
        //        if (!playableGraph.IsValid())
        //        {
        //            Debug.Log(playableGraph + "为空");
        //            return default;
        //        }
        //        //问题是baseCameraPosition拿不到值
        //        //var startPos = ((CameraFocusClip)(clips[0].asset)).template.baseCameraPosition;
        //        var startPos = Camera.main.transform.position;//可能是因为已经将轨道销毁的原因所以拿不到

        //        var tailDataClip = ((clips[clips.Length - 1]));
        //        var endCameraFocusClip = ((CameraFocusClip)(tailDataClip.asset));

        //        Transform target = endCameraFocusClip.focus.Resolve(playableGraph.GetResolver());
        //        var endPos = target.position;

        //        Vector3 offsetPos = Vector3.zero;
        //        Debug.Log("摄像机的原始值为"+startPos);
        //        Debug.Log("目标位置的原始值为" + endPos);
        //        if (!endCameraFocusClip.template.LockX)
        //        {
        //            offsetPos.x = startPos.x - endPos.x;
        //        }
        //        if (!endCameraFocusClip.template.LockY)
        //        {
        //            offsetPos.y = startPos.y - endPos.y;
        //        }
        //        return offsetPos;//尾部的坐标系
        //    }
        //}
        #endregion
    }
}
