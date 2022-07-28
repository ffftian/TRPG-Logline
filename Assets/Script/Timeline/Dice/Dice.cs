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
        public Text diceText;

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


            //晃动


            //diceTrans.anchoredPosition = new Vector2(UnityEngine.Random.Range(-shake, shake), UnityEngine.Random.Range(-shake, shake));

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

