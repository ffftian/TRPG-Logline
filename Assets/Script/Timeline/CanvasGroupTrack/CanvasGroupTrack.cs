using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Miao
{
    [TrackColor(0.45f, 0.2f, 0.2f)]
    [TrackBindingType(typeof(CanvasGroup))]
    [TrackClipType(typeof(CanvasGroupClip))]
    public class CanvasGroupTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<CanvasGroupMixBehaviour>.Create(graph, inputCount);
        }
    }
}
