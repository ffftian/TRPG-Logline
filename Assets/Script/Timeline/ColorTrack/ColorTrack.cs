using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


namespace MiaoTween
{
    [TrackBindingType(typeof(Color))]
    [TrackClipType(typeof(ColorClip))]
    abstract public class ColorTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<ColorMixBehaviour>.Create(graph, inputCount);
        }
    }
}
