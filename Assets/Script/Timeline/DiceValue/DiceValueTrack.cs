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
    [TrackClipType(typeof(DiceValueClip))]//Clip = PlayableAsset
    [TrackBindingType(typeof(DiceValue))]
    public class DiceValueTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<DiceValueMixBehaviour> scriptPlayable = ScriptPlayable<DiceValueMixBehaviour>.Create(graph, inputCount);
            return scriptPlayable;
        }
    }
}
