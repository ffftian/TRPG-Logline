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
    public class DiceRandomBehaviour : PlayableBehaviour
    {
        //[NonSerialized]
        //public Image mask;
        [NonSerialized]
        public Dice component;
        /// <summary>
        /// 玩家特定技能值
        /// </summary>
        [Tooltip("玩家鉴定技能值")]
        public int playSkillValue = 50;
        [Tooltip("随机到的技能值")]
        public int finishValue = 50;
        /// <summary>
        /// 骰子随机到的位置
        /// </summary>
        [Tooltip("骰子入场随机")]
        public Vector2 enterOffset = new Vector2(-600, 0);
        [Range(0f, 1f)]
        [Tooltip("在多少时间时骰子停止")]
        public float stopPercentage = 0.8f;


        [Tooltip("缩放效果，请用时间轴编辑进行播放间调整")]
        public Vector3 scale = Vector3.one;
        [Tooltip("旋转圈速速度")]
        public float rotationSpeed = 10;
        //private int nodeIndex;
        private Vector2 basePos;
        private Vector2 movePos;

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {

            base.OnBehaviourPlay(playable, info);
            //if (component != null)
            //{
            //    InitComponent();
            //}
        }
        public override void OnPlayableDestroy(Playable playable)
        {
            Hide();
            component?.Finish(finishValue);
            component?.StageClear();
            base.OnPlayableDestroy(playable);
        }

        private void InitComponent()
        {
            component.playSkillValue = playSkillValue;
            basePos = component.diceTransform.anchoredPosition;
            movePos = component.diceTransform.anchoredPosition + enterOffset;
        }
        void Show()
        {
            component.Show();
            //mask?.gameObject.SetActive(true);

        }
        void Hide()
        {
            if (component != null)
            {
                component.diceTransform.localScale = Vector3.one;
                component.diceTransform.eulerAngles = Vector3.zero;
                component.diceTransform.anchoredPosition = basePos;
                component.Hide();
                component.StageClear();
            }
            //mask?.gameObject.SetActive(false);
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

            float progress = (float)(playable.GetTime() / playable.GetDuration());
            float advance = Mathf.Lerp(0, progress / stopPercentage, 1);
            component.diceTransform.anchoredPosition = Vector2.Lerp(movePos, basePos, advance);
            component.diceTransform.localScale = scale;

            if (progress < stopPercentage)
            {
                component.Random();
                component.diceTransform.Rotate(0, 0, -rotationSpeed);
                component.StageClear();
            }
            else
            {
                component.diceTransform.eulerAngles = Vector3.Lerp(component.diceTransform.eulerAngles, Vector3.zero,0.5f);
                component.Finish(finishValue);
                component.SetStage(finishValue);
            }
            #region 显示处理
            if (progress <= 0.01f)
            {
                component.diceTransform.localScale = Vector3.one;
                component.diceTransform.eulerAngles = Vector3.zero;
                component.diceTransform.anchoredPosition = basePos;
            }
            else if (progress <= 0.99f)
            {
                Show();
            }
            else
            {
                Hide();
            }
            #endregion

        }


    }
}
