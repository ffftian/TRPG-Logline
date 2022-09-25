using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

namespace PostProcess
{
    public class PostProcessClip<TBehaviour, TData> : PlayableAsset, ITimelineClipAsset where TBehaviour : PostProcessData<TData>, new() where TData : new()
    {
        public TData data;

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TBehaviour>.Create(graph);
            playable.GetBehaviour().data = data;
            return playable;
        }

    }
}

