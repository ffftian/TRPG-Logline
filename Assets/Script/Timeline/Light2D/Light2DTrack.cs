using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

namespace MiaoTween
{
    [TrackBindingType(typeof(Light2D))]
    [TrackClipType(typeof(Light2DClip))]
    public class Light2DTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<Light2DMixBehaviour>.Create(graph, inputCount);
        }
    }
}
