using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace MiaoTween
{
    public class Light2DClip : PlayableAsset, ITimelineClipAsset
    {
        //[SerializeField]
        public Light2DSpotBehaviour template;
        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<Light2DSpotBehaviour>.Create(graph, template);
            return playable;
        }
    }
}
