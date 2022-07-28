using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
namespace Experimental
{
    [Serializable]
    public class EyeLookClip : PlayableAsset, ITimelineClipAsset
    {
        public EyeLookBehaviour template = new EyeLookBehaviour();
        public ClipCaps clipCaps => ClipCaps.All;
        public ExposedReference<Transform> target;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<EyeLookBehaviour>.Create(graph, template);

            template.target = target.Resolve(graph.GetResolver());
            return playable;
        }

    }
}

