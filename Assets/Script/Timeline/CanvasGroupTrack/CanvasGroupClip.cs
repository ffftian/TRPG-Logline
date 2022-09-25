using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MiaoTween
{
    public class CanvasGroupClip : PlayableAsset, ITimelineClipAsset
    {
        public CanvasGroupBehaviour template;
        public float alpha = 1;

        public ClipCaps clipCaps => ClipCaps.Blending;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<CanvasGroupBehaviour>.Create(graph, template);
            playable.GetBehaviour().alpha= alpha;
            return playable;
        }
    }
}
