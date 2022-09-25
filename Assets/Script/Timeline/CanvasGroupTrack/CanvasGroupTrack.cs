using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MiaoTween
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
