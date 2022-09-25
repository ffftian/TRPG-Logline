using Sirenix.OdinInspector;
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
    public class MovieScaleLineClip : PlayableAsset, ITimelineClipAsset
    {
        
        //public MovieScaleLineBehaviour template;
        public Vector2 top = Vector2.zero;
        public Vector2 bottom = Vector2.zero;
        public ClipCaps clipCaps => ClipCaps.Blending;
        /// <summary>
        /// 创建一个行为，当数值更改时，就会触发
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var scriptPlayable = ScriptPlayable<MovieScaleLineBehaviour>.Create(graph);
            MovieScaleLineBehaviour movieScaleLineBehaviour = scriptPlayable.GetBehaviour();
            //var scriptPlayable = ScriptPlayable<MovieScaleLineBehaviour>.Create(graph, template);
            movieScaleLineBehaviour.top = top;
            movieScaleLineBehaviour.bottom = bottom;
            return scriptPlayable;
        }

    }
}
