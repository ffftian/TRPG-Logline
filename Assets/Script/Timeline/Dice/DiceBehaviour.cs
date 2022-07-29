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
    [Serializable]
    public class DiceBehaviour : PlayableBehaviour
    {
        [NonSerialized]
        public Image mask;
        [NonSerialized]
        public Dice component;
        /// <summary>
        /// 玩家特定技能值
        /// </summary>
        [Tooltip("随机开始时的技能值")]
        public int playSkillValue = 50;
        [Tooltip("随机完成时的技能值")]
        public int finishValue = 50;

        [Tooltip("骰子执行投掷时的晃动幅度,可以通过点击小轨道符号来按曲线定义shake在timeline中的强度")]
        public float shake = 10f;
        [Tooltip("晃动强度比值")]
        public float strength = 1f;
        /// <summary>
        /// 在多少间隔前
        /// </summary>
        [Tooltip("多少值前显示完成比值")]
        public int ShowfinishValueOffSet = 1;
        /// <summary>
        /// 间隔多少时间刷新一次骰子数值
        /// </summary>
        [Range(0.04f,1)]
        public float duration = 0.1f;

        public float alpha = 1;

        private int nodeIndex;
        private double hideTime;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            
            base.OnBehaviourPlay(playable, info);
            if (component != null)
            {
                InitComponent();
            }
            hideTime = playable.GetDuration() - duration;
        }
        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
            component?.Hide();
            component?.Finish(finishValue);
           
        }

        private void InitComponent()
        {
            nodeIndex = 0;
            component.playSkillValue = playSkillValue;
        }
        void Show()
        {
            component.Show();
            mask?.gameObject.SetActive(true);

        }
        void Hide()
        {
            component.Hide();
            mask?.gameObject.SetActive(false);
        }

        /// <summary>
        /// 基于脚本的
        /// </summary>
        /// <param name="playable"></param>
        /// <param name="info"></param>
        /// <param name="playerData"></param>
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            //playable.SetTime(time);
            if (component == null)//没有获得外部脚本时的逻辑
            {
                component = playerData as Dice;
                if (component == null) return;
                InitComponent();
            }
            component.alpha =  alpha;
            if (playable.GetTime() < nodeIndex)
            {
                Show();
            }

            if (playable.GetTime() > nodeIndex * duration)
            {
                nodeIndex++;
                component.Random(shake, strength);
            }
            //Debug.Log($"总间隔{playable.GetDuration()}+当前间隔{(nodeIndex + ShowfinishValueOffSet) * duration}");
            if (playable.GetDuration() <= (nodeIndex + ShowfinishValueOffSet) * duration)
            {
                nodeIndex = 9999;
                component.Finish(finishValue);
            }
            if (playable.GetTime() >= hideTime)
            {
                Hide();
            }

        }
       

    }
}
