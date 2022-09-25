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
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(AnimationCurvePositonClip))]
    public class AnimationCurvePositonTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<AnimationCurvePositonMixBehaviour>.Create(graph, inputCount);
        }
    }
}

