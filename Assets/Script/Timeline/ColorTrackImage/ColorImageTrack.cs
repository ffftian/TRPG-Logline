using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace MiaoTween
{
    [TrackColor(0.2f, 0.2f, 0.2f)]
    [TrackBindingType(typeof(Graphic))]
    [TrackClipType(typeof(ColorClip))]
    public class ColorImageTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<ColorImageMixBehaviour>.Create(graph, inputCount);
        }
    }
}
