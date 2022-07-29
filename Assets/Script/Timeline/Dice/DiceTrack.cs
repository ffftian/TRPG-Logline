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
    [TrackClipType(typeof(DiceClip))]//Clip = PlayableAsset
    [TrackBindingType(typeof(Dice))]
    public class DiceTrack : TrackAsset
    {
        public ExposedReference<Image> mask;
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            ScriptPlayable<DiceBehaviour> dice = (ScriptPlayable<DiceBehaviour>)(base.CreatePlayable(graph, gameObject, clip));
            DiceBehaviour diceBehaviour = dice.GetBehaviour();
            diceBehaviour.mask = mask.Resolve(graph.GetResolver());
            return dice;
        }

        //protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        //{
        //    ScriptPlayable<DiceBehaviour> scriptPlayable = ScriptPlayable<DiceBehaviour>.Create(graph);
        //    DiceBehaviour dice = scriptPlayable.GetBehaviour();
        //    dice.mask = mask.Resolve(graph.GetResolver());
        //    dice.mask.color = value;
        //    return scriptPlayable;
        //    //return base.CreatePlayable(graph, gameObject, clip);
        //}

        //public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        //{

        //}
    }
}
