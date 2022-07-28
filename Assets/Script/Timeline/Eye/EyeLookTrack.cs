using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace Experimental
{
    /// <summary>
    /// 脚本类
    /// </summary>
    [TrackColor(0.7f, 0.2f, 0.2f)]
    [TrackBindingType(typeof(MiaoEyeLook))]
    [TrackClipType(typeof(EyeLookClip))]
    public class EyeLookTrack : TrackAsset
    {


        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return base.CreateTrackMixer(graph, go, inputCount);
        }
    }
}