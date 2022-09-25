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
    [Serializable]
    public class ExposedReferenceDiceValue : ExposedReferenceHolder<DiceValue>
    {

    }


    [TrackColor(0.8f, 0.2f, 0.2f)]
    [TrackClipType(typeof(DiceValueMultipleClip))]//Clip = PlayableAsset
    public class DiceValueMultipleTrack : TrackAsset
    {
        public ExposedReferenceDiceValue[] exposedReferenceDices;

        protected override void OnCreateClip(TimelineClip clip)
        {
            DiceValueMultipleClip diceClip = (clip.asset as DiceValueMultipleClip);
            diceClip.diceParameters = new DiceValueParameter[exposedReferenceDices.Length];
            for (int i = 0; i < exposedReferenceDices.Length; i++)
            {
                diceClip.diceParameters[i] = new DiceValueParameter();
            }

            base.OnCreateClip(clip);
        }

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<DiceValueMultipleMixBehaviour> scriptPlayable = ScriptPlayable<DiceValueMultipleMixBehaviour>.Create(graph, inputCount);
            DiceValueMultipleMixBehaviour diceBehaviour = scriptPlayable.GetBehaviour();
            diceBehaviour.componets = new DiceValue[exposedReferenceDices.Length];
            for (int i = 0; i < exposedReferenceDices.Length; i++)
            {
                diceBehaviour.componets[i] = exposedReferenceDices[i].ExposedReference.Resolve(graph.GetResolver());
            }
            diceBehaviour.Init();
            return scriptPlayable;
        }
    }
}
