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
    public class DiceRollMultipleClip : PlayableAsset, ITimelineClipAsset
    {
        public DiceRollParameter[] diceParameters;
        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<DiceRollMultipleBehaviour> playable =  ScriptPlayable<DiceRollMultipleBehaviour>.Create(graph);
            playable.GetBehaviour().diceParameters = diceParameters;
            return playable;
        }
    }
}
