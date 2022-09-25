using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Dice
{
    public class DiceValueClip : PlayableAsset, ITimelineClipAsset//Asset
    {
        public DiceValueParameter diceValueParameter;
        public ClipCaps clipCaps => ClipCaps.Blending;
        /// <summary>
        /// 创建时可以改为创建Playable的
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {

            ScriptPlayable<DiceValueBehaviour> playable = ScriptPlayable<DiceValueBehaviour>.Create(graph);
            DiceValueBehaviour behaviour = playable.GetBehaviour();
            behaviour.diceParameter = diceValueParameter;
            return playable;
        }

    }
}
