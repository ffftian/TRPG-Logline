using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class DiceValue : MonoBehaviour
    {

        [Range(0, 100)]
        public int diceRange = 100;
        public RectTransform rectTransform;
        public Image diceImage;
        public TMP_Text diceNumber;
        public TMP_Text diceFormula;
        public Image mask;


        public float diceNumberSize
        {
            get
            {
                return diceNumber.fontSize;
            }
            set
            {
                diceNumber.fontSize =value;
            }
        }

    }
}

