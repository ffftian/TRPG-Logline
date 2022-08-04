using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Dice
{
    public class Dice : MonoBehaviour
    {
        [Range(0, 99)]
        public int diceRange = 99;
        public int playSkillValue;
        public RectTransform diceTrans;
        public Image diceImage;
        public Text diceText;
        public float alpha
        {
            set
            {
                diceImage.color = new Color(diceImage.color.r, diceImage.color.g, diceImage.color.b,value);
                diceText.color = new Color(diceText.color.r, diceText.color.g, diceText.color.b, value);
            }
        }
  

        public void Show()
        {
            diceTrans.gameObject.SetActive(true);
        }
        public void Hide()
        {
            diceTrans.gameObject.SetActive(false);
        }

        


        /// <summary>
        /// 随机数值一次
        /// </summary>

        public void Random(float shake,float strength)
        {
            //随机数值
            int value = UnityEngine.Random.Range(0, diceRange);
            diceText.text = $"{value}/{playSkillValue}";
            diceText.color = Color.black;
            float x =  Mathf.Cos(shake) + Mathf.Sin(shake);
            float y = -Mathf.Sin(shake) + Mathf.Cos(shake);
            diceTrans.anchoredPosition = new Vector2(x, y)* shake* strength;
        }
        public void Finish(int finishValue)
        {
            diceText.text = $"{finishValue}/{playSkillValue}";
            if (finishValue > playSkillValue)
            {
                diceText.color = Color.red;
            }
            else
            {
                diceText.color = Color.blue;
            }
            diceTrans.anchoredPosition = Vector2.zero;
        }



    }
}

