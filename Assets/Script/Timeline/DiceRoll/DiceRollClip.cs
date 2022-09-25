using Sirenix.OdinInspector;
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
    public class DiceRollClip : PlayableAsset, ITimelineClipAsset//Asset
    {
        public DiceRollParameter diceParameter = new DiceRollParameter();
        public ClipCaps clipCaps => ClipCaps.Blending;
        /// <summary>
        /// 创建时可以改为创建Playable的
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            //ScriptPlayable<DiceProBehaviour> playable = ScriptPlayable<DiceProBehaviour>.Create(graph, template);
            //return playable;

            ScriptPlayable<DiceRollBehaviour> playable = ScriptPlayable<DiceRollBehaviour>.Create(graph);
            DiceRollBehaviour diceProBehaviour = playable.GetBehaviour();
            diceProBehaviour.diceParameter = diceParameter;
            return playable;
        }

    }
}
