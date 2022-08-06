﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Dice
{
    public class DiceRandomClip : PlayableAsset, ITimelineClipAsset//Asset
    {
        //这个就是节点最大可用长度
        //public override double duration
        //{
        //    get
        //    {
        //        return template.duration;
        //    }
        //}

        [SerializeField] public DiceRandomBehaviour template;
        [SerializeField] public bool loop = true;

        public ClipCaps clipCaps => ClipCaps.ClipIn | (loop ? ClipCaps.Looping : ClipCaps.None);

        

        /// <summary>
        /// 创建时可以改为创建Playable的
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<DiceRandomBehaviour> playable = ScriptPlayable<DiceRandomBehaviour>.Create(graph, template);
            return playable;
        }

    }
}
