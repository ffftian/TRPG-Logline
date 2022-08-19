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

        [Range(0, 100)]
        public int diceRange = 100;
        public int playSkillValue;
        public RectTransform diceTransform;
        public Image diceImage;
        public Text diceText;
        public Text stageText;
        public float alpha
        {
            set
            {
                diceImage.color = new Color(diceImage.color.r, diceImage.color.g, diceImage.color.b, value);
                diceText.color = new Color(diceText.color.r, diceText.color.g, diceText.color.b, value);
            }
        }


        public void Show()
        {
            diceTransform.gameObject.SetActive(true);
        }
        public void Hide()
        {
            diceTransform.gameObject.SetActive(false);
        }


        public void Random()
        {
            int value = UnityEngine.Random.Range(1, diceRange);
            diceText.text = $"{value}/{playSkillValue}";
            diceText.color = Color.black;
        }

        [Obsolete("这随机随机产生的演出效果不是很出色")]
        public void RandomCircular(float shake, float strength)
        {
            //随机数值
            int value = UnityEngine.Random.Range(0, diceRange);
            diceText.text = $"{value}/{playSkillValue}";
            diceText.color = Color.black;
            float x = Mathf.Cos(shake) + Mathf.Sin(shake);
            float y = -Mathf.Sin(shake) + Mathf.Cos(shake);
            diceTransform.anchoredPosition = new Vector2(x, y) * shake * strength;
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
            diceTransform.anchoredPosition = Vector2.zero;
        }
        public void StageClear()
        {
            stageText.text = "";
        }

        public void SetStage(int finishValue)
        {
            if (finishValue == 0 || finishValue > 95)
            {
                stageText.text = "大失败！！！";
                return;
            }
            if (finishValue <= 5 && finishValue <= playSkillValue)
            {
                stageText.text = "大成功!!!";
            }
            else if (finishValue <= playSkillValue / 5)
            {
                stageText.text = "极难成功!!";
            }
            else if (finishValue <= playSkillValue / 2)
            {
                stageText.text = "困难成功!";
            }
            else if (finishValue <= playSkillValue)
            {
                stageText.text = "成功";
            }
            else if (finishValue > playSkillValue)
            {
                stageText.text = "失败";
            }
        }

    }
}

