using System;
using UnityEngine;

namespace Dice
{
    [Serializable]
    public class DiceValueParameter
    {
        [Tooltip("骰子结果值")]
        public int diceValue = 1;
        [Tooltip("骰子随机区间")]
        public Vector2Int diceRanage = new Vector2Int(1, 7);
        [Tooltip("字体大小偏移")]
        public float NumberSizeOffset = 0;
        [Tooltip("震动效果")]
        public float shake = 0;
        [Tooltip("骰子计算式")]
        public string diceFormula = "";

#if UNITY_EDITOR
        [Sirenix.OdinInspector.Button("预设骰子开始")]
        public void DefaultStart()
        {
            shake = 10f;
            NumberSizeOffset = 16f;
            diceFormula = "1d6";
        }
        [Sirenix.OdinInspector.Button("预设骰子结束")]
        public void DefaultEnd()
        {
            diceFormula = "1d6";
        }
#endif
    }
}
