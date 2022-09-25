using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Dice
{
    public class DiceRoll : MonoBehaviour
    {

        [Range(0, 100)]
        public int diceRange = 100;
        public int playSkillValue;
        public RectTransform rectTransform;
        public Image diceImage;
        public TMP_Text diceText;
        public TMP_Text stageText;
        public Image mask;


        public float Angle
        {
            get
            {
                return rectTransform.eulerAngles.z;
            }
            set
            {
                rectTransform.eulerAngles = new Vector3(0, 0, value);
            }
        }
        public float NumberScale
        {
            get
            {
                return diceText.transform.localScale.z;
            }
            set
            {
                diceText.transform.localScale = new Vector3(diceText.transform.localScale.x, diceText.transform.localScale.y, value);
            }
        }


        public float Alpha
        {
            set
            {
                diceImage.color = new Color(diceImage.color.r, diceImage.color.g, diceImage.color.b, value);
                diceText.color = new Color(diceText.color.r, diceText.color.g, diceText.color.b, value);
            }
        }


        public void Show()
        {
            rectTransform.gameObject.SetActive(true);
        }
        public void Hide()
        {
            rectTransform.gameObject.SetActive(false);
        }


        public void Random()
        {
            int value = UnityEngine.Random.Range(1, diceRange);
            diceText.text = $"{value}/{playSkillValue}";
            diceText.color = Color.black;
        }


        public void FinishShow(int finishValue)
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
            rectTransform.anchoredPosition = Vector2.zero;
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

        //public void ProcessFrameDice(float[] inputWeight, DiceParameter diceParameter, float baseAngle, Vector3 baseAnchoredPosition)
        //{
        //    int inputCount = playable.GetInputCount();
        //    if (inputCount == 0)
        //    {
        //        mask.gameObject.SetActive(false);
        //    }

        //    float angle = 0;
        //    Vector3 positon = Vector3.zero;
        //    float textScale = 1;
        //    bool noWeight = true;
        //    double minTime = 100;
        //    for (int i = 0; i < inputCount; i++)
        //    {
        //        float inputWeight = playable.GetInputWeight(i);
        //        var inputPlayable = (ScriptPlayable<DiceProBehaviour>)playable.GetInput(i);
        //        //DiceProBehaviour diceProBehaviour = inputPlayable.GetBehaviour();
        //        angle += inputWeight * diceProBehaviour.angle;
        //        positon += inputWeight * diceProBehaviour.enterOffset;
        //        float x = UnityEngine.Random.Range(-diceProBehaviour.shake, diceProBehaviour.shake);
        //        float y = UnityEngine.Random.Range(-diceProBehaviour.shake, diceProBehaviour.shake);
        //        positon += inputWeight * new Vector3(x, y, 0);


        //        if (inputWeight == 1)
        //        {
        //            FinishShow(diceProBehaviour.finishValue);
        //            SetStage(diceProBehaviour.finishValue);

        //            if (diceProBehaviour.showStage)
        //            {
        //                stageText.gameObject.SetActive(true);
        //            }
        //            else
        //            {
        //                gameObject.SetActive(false);
        //            }
        //        }
        //        else if (inputWeight > 0)
        //        {
        //            Random();

        //        }
        //        if (inputWeight > 0)
        //        {
        //            double time = inputPlayable.GetTime();
        //            if (time < minTime)
        //            {
        //                minTime = time;
        //                playSkillValue = diceProBehaviour.playSkillValue;
        //            }
        //            mask.gameObject.SetActive(true);
        //            gameObject.SetActive(true);
        //            noWeight = false;
        //        }
        //    }

        //    if (noWeight)
        //    {
        //        gameObject.SetActive(false);
        //        mask.gameObject.SetActive(false);
        //    }
        //    Angle = baseAngle + angle;
        //    transform.position = baseAnchoredPosition + positon;
        //    NumberScale = textScale;
        //}



        public void ProcessFrameDice(Playable playable,DiceRollParameter diceParameter, float baseAngle, Vector3 basePositon)
        {

        }

    }
}

