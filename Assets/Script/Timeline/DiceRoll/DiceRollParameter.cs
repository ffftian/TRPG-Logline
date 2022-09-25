using System;
using UnityEngine;

namespace Dice
{
    [Serializable]
    public class DiceRollParameter
    {
        [Tooltip("结束时的角度")]
        public float angle;
        [Tooltip("玩家鉴定技能值")]
        public int playSkillValue = 0;
        [Tooltip("随机到的技能值")]
        public int finishValue = 0;
        [Tooltip("骰子位置")]
        public Vector2 enterOffset = new Vector2(0, 0);
        [Tooltip("技能值缩放")]
        public float textScale = 1;
        [Tooltip("是否显示结算")]
        public bool showStage = true;
        [Tooltip("投掷中不规则晃动")]
        public float shake;

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button("预设骰子起始")]
        public void DefaultStart()
        {
            showStage = false;
            enterOffset = new Vector2(-800, 0);
        }
        [Sirenix.OdinInspector.Button("预设骰子结束")]
        public void DefaultEnd()
        {

            showStage = true;
            angle = -720;
        }
#endif
    }
}
