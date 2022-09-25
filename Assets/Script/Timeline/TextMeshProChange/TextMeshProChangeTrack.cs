using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MiaoTween
{

    [TrackBindingType(typeof(TMP_Text))]
    [TrackClipType(typeof(TextMeshProChangeClip))]
    public class TextMeshProChangeTrack : TrackAsset
    {
#if UNITY_EDITOR
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<TextMeshProChangeMixBehaviour>.Create(graph, inputCount);
        }
#endif
    }
}

