using Spine.Unity;
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
    [TrackColor(0.2f, 0.2f, 0.2f)]
    [TrackBindingType(typeof(SkeletonAnimation))]
    [TrackClipType(typeof(ColorBehaviour))]
    public class ColorSpineTrack : TrackAsset
    {

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<ColorSpineMixBehaviour>.Create(graph, inputCount);
        }

    }
}