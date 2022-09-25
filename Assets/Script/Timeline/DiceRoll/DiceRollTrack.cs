using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Dice
{
    [TrackColor(0.6f, 0.2f, 0.2f)]
    [TrackClipType(typeof(DiceRollClip))]//Clip = PlayableAsset
    [TrackBindingType(typeof(DiceRoll))]
    public class DiceRollTrack : TrackAsset
    {

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<DiceRollMixBehaviour> scriptPlayable = ScriptPlayable<DiceRollMixBehaviour>.Create(graph, inputCount);
            return scriptPlayable;
        }
    }
}
