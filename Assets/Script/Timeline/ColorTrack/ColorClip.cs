using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MiaoTween
{
    public class ColorClip : PlayableAsset, ITimelineClipAsset
    {
        public ColorBehaviour template;
        public Color color = Color.white;
        public ClipCaps clipCaps => ClipCaps.Blending;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ColorBehaviour>.Create(graph, template);
            playable.GetBehaviour().color = color;
            return playable;
        }
    }
}

