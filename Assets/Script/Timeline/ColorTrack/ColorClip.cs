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

