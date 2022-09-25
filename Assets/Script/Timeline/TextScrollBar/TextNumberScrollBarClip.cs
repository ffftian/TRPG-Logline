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
    public class TextNumberScrollBarClip : PlayableAsset, ITimelineClipAsset
    {
        public int NumberValue;
        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TextNumberScrollBarBehaviour>.Create(graph);
            TextNumberScrollBarBehaviour textScrollBarBehaviour = playable.GetBehaviour();
            textScrollBarBehaviour.Number = NumberValue;
            return playable;
        }
    }
}
