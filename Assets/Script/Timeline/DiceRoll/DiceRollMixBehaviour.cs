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
    public class DiceRollMixBehaviour : PlayableBehaviour
    {
        DiceRoll component;
        float baseAngle;
        Vector2 baseAnchoredPosition;
        float baseTextScale = 1;
        bool baseActive;
        public override void OnPlayableDestroy(Playable playable)
        {
            // base.OnPlayableDestroy(playable);
            if (component != null)
            {
                component.Angle = baseAngle;
                component.rectTransform.anchoredPosition = baseAnchoredPosition;
                component.NumberScale = baseTextScale;

                component.gameObject.SetActive(baseActive);
                component.mask.gameObject.SetActive(baseActive);
            }
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (component == null)
            {
                component = playerData as DiceRoll;
                baseAngle = component.Angle;
                baseAnchoredPosition = component.rectTransform.anchoredPosition;
                baseActive = component.gameObject.activeSelf;
            }

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
                var inputPlayable = (ScriptPlayable<DiceRollBehaviour>)playable.GetInput(i);
                DiceRollParameter diceProBehaviour = inputPlayable.GetBehaviour().diceParameter;


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
    }
}
