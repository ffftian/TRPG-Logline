using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace MiaoTween
{

    [TrackColor(0.2f, 0.2f, 0.2f)]
    [TrackBindingType(typeof(SpriteRenderer))]
    [TrackClipType(typeof(ColorClip))]
    public class ColorSpriteTrack : TrackAsset
    {

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<ColorSpriteMixBehaviour>.Create(graph, inputCount);
        }

    }

}