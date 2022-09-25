using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Dice
{
    public class DiceRollMultipleMixBehaviour : PlayableBehaviour
    {
        //public DiceRoll[] exposedReferenceDices;
        public DiceRoll[] componets;
        float[] baseAngle;
        Vector2[] baseAnchoredPosition;
        float[] baseNumberScale;
        bool[] baseActive;

        public void Init()
        {
            int length = componets.Length;
            baseAngle = new float[length];
            baseAnchoredPosition = new Vector2[length];
            baseNumberScale = new float[length];
            baseActive = new bool[length];

            for (int j = 0; j < componets.Length; j++)
            {
                baseAngle[j] = componets[j].Angle;
                baseAnchoredPosition[j] = componets[j].rectTransform.anchoredPosition;
                baseNumberScale[j] = componets[j].NumberScale;
                baseActive[j] = componets[j].gameObject.activeSelf;
            }
        }

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
            for (int i = 0; i < componets.Length; i++)
            {
                SetValue(playable, i, componets[i], baseAngle[i], baseAnchoredPosition[i]);
            }
        }
        public void SetValue(Playable playable,int diceParameterPtr, DiceRoll component, float baseAngle, Vector2 baseAnchoredPosition)
        {
            int inputCount = playable.GetInputCount();
            if (inputCount == 0)
            {
                component.mask.gameObject.SetActive(false);
            }
            float angle = 0;
            Vector2 AnchoredPositon = Vector3.zero;
            float textScale = 1;
            bool noWeight = true;
            double minTime = 100;
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<DiceRollMultipleBehaviour>)playable.GetInput(i);
                DiceRollParameter diceProBehaviour = inputPlayable.GetBehaviour().diceParameters[diceParameterPtr];


                angle += inputWeight * diceProBehaviour.angle;
                AnchoredPositon += inputWeight * diceProBehaviour.enterOffset;
                float x = UnityEngine.Random.Range(-diceProBehaviour.shake, diceProBehaviour.shake);
                float y = UnityEngine.Random.Range(-diceProBehaviour.shake, diceProBehaviour.shake);
                AnchoredPositon += inputWeight * new Vector2(x, y);


                if (inputWeight == 1)
                {
                    component.FinishShow(diceProBehaviour.finishValue);
                    component.SetStage(diceProBehaviour.finishValue);

                    if (diceProBehaviour.showStage)
                    {
                        component.stageText.gameObject.SetActive(true);
                    }
                    else
                    {
                        component.stageText.gameObject.SetActive(false);
                    }
                }
                else if (inputWeight > 0)
                {
                    component.Random();

                }
                if (inputWeight > 0)
                {
                    double time = inputPlayable.GetTime();
                    if (time < minTime)
                    {
                        minTime = time;
                        component.playSkillValue = diceProBehaviour.playSkillValue;
                    }
                    component.mask.gameObject.SetActive(true);
                    component.gameObject.SetActive(true);
                    noWeight = false;
                }

            }
            if (noWeight)
            {
                component.gameObject.SetActive(false);
                component.mask.gameObject.SetActive(false);
            }
            component.Angle = baseAngle + angle;
            component.rectTransform.anchoredPosition = baseAnchoredPosition + AnchoredPositon;
            component.NumberScale = textScale;
        }


        public override void OnPlayableDestroy(Playable playable)
        {
            for (int j = 0; j < componets.Length; j++)
            {
                componets[j].Angle = baseAngle[j];
                componets[j].rectTransform.anchoredPosition = baseAnchoredPosition[j];
                componets[j].NumberScale = baseNumberScale[j];
                componets[j].gameObject.SetActive(baseActive[j]);
            }

            base.OnPlayableDestroy(playable);
        }
    }
}
