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
    public class DiceValueMixBehaviour : PlayableBehaviour
    {
        DiceValue component;
        //float baseAngle;
        Vector2 baseAnchoredPosition;
        float baseNumberSize = 0;
        bool baseActive;

        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (component != null)
                {
                    //volume.Angle = baseAngle;
                    component.rectTransform.anchoredPosition = baseAnchoredPosition;
                    component.diceNumberSize = baseNumberSize;

                    component.gameObject.SetActive(baseActive);
                    component.mask.gameObject.SetActive(baseActive);
                }
            }
#endif
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (component == null)
            {
                component = playerData as DiceValue;
                baseAnchoredPosition = component.rectTransform.anchoredPosition;
                baseNumberSize = component.diceNumberSize;
                baseActive = component.gameObject.activeSelf;
            }

            int inputCount = playable.GetInputCount();
            double minTime = 100;
            bool noWeight = true;

            Vector2 randPositon = Vector2.zero;
            float diceNumberSize = 0;
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<DiceValueBehaviour>)playable.GetInput(i);
                DiceValueParameter diceProBehaviour = inputPlayable.GetBehaviour().diceParameter;
                float x = UnityEngine.Random.Range(-diceProBehaviour.shake, diceProBehaviour.shake);
                float y = UnityEngine.Random.Range(-diceProBehaviour.shake, diceProBehaviour.shake);
                randPositon += inputWeight * new Vector2(x, y);
                diceNumberSize += inputWeight * diceProBehaviour.NumberSizeOffset;

                if (inputWeight > 0)
                {
                    double time = inputPlayable.GetTime();
                    if (time < minTime)
                    {
                        minTime = time;
                        component.diceFormula.text = diceProBehaviour.diceFormula;
                        component.mask.gameObject.SetActive(true);
                        component.gameObject.SetActive(true);
                        noWeight = false;

                        if (inputWeight == 1)
                        {
                            component.diceNumber.text = diceProBehaviour.diceValue.ToString();
                        }

                        else if (inputWeight != 1)
                        {
                            int number = UnityEngine.Random.Range(diceProBehaviour.diceRanage.x, diceProBehaviour.diceRanage.y);
                            component.diceNumber.text = number.ToString();
                        }
                    }
                }
                if (noWeight)
                {
                    component.gameObject.SetActive(false);
                    component.mask.gameObject.SetActive(false);
                }
                component.rectTransform.anchoredPosition = baseAnchoredPosition + randPositon;
                component.diceNumberSize = baseNumberSize + diceNumberSize;

            }
        }
    }
}
