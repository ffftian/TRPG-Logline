using Spine.Unity;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace MiaoTween
{
    [TrackColor(0.2f, 0.2f, 0.2f)]
    [TrackBindingType(typeof(SkeletonAnimation))]
    [TrackClipType(typeof(ColorClip))]
    public class ColorSpineTrack : TrackAsset
    {

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<ColorSpineMixBehaviour>.Create(graph, inputCount);
        }

    }
}