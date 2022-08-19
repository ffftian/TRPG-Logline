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
