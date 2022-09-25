using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Dice
{
    [Serializable]
    public class ExposedReferenceDiceRoll : ExposedReferenceHolder<DiceRoll>
    {

    }


    //[UnityEditor.CustomEditor()]
    [TrackColor(0.6f, 0.2f, 0.2f)]
    [TrackClipType(typeof(DiceRollMultipleClip))]//Clip = PlayableAsset
    public class DiceRollMultipleTrack : TrackAsset
    {
        public ExposedReferenceDiceRoll[] exposedReferenceDices;


        protected override void OnCreateClip(TimelineClip clip)
        {
            DiceRollMultipleClip diceClip = (clip.asset as DiceRollMultipleClip);
            diceClip.diceParameters = new DiceRollParameter[exposedReferenceDices.Length];
            for (int i = 0; i < exposedReferenceDices.Length; i++)
            {
                diceClip.diceParameters[i] = new DiceRollParameter();
             }
            base.OnCreateClip(clip);

        }

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<DiceRollMultipleMixBehaviour> scriptPlayable = ScriptPlayable<DiceRollMultipleMixBehaviour>.Create(graph, inputCount);
            DiceRollMultipleMixBehaviour diceProMultipleMixBehaviour = scriptPlayable.GetBehaviour();
            diceProMultipleMixBehaviour.componets = new DiceRoll[exposedReferenceDices.Length];
            for (int i = 0; i < exposedReferenceDices.Length; i++)
            {
                diceProMultipleMixBehaviour.componets[i] = exposedReferenceDices[i].ExposedReference.Resolve(graph.GetResolver());
            }
            diceProMultipleMixBehaviour.Init();
            return scriptPlayable;
        }
    }
}

