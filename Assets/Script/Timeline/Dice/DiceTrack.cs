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
    [TrackColor(0.6f, 0.2f, 0.2f)]
    [TrackClipType(typeof(DiceClip))]//Clip = PlayableAsset
    [TrackBindingType(typeof(Dice))]
    public class DiceTrack : TrackAsset
    {
        public int 轨道数值;





        /// <summary>
        /// 只要置入一个Cilp，都会触发一次，混合可能是会额外创建轨道的，不要写
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="go"></param>
        /// <param name="inputCount"></param>
        /// <returns></returns>
        //public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        //{
        //    //他的默认调用就是   return Playable.Create(graph, inputCount);
           
        //    var scriptPlayable = ScriptPlayable<DiceBehaviour>.Create(graph, inputCount);

        //    return scriptPlayable;
        //}

    }
}
