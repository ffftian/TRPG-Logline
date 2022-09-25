using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Dice
{
    public class DiceValueMultipleMixBehaviour : PlayableBehaviour
    {
        public DiceValue[] componets;
        public Vector2[] baseAnchoredPosition;
        public float[] baseNumberSize;


        public void Init()
        {
            int length = componets.Length;
            baseAnchoredPosition = new Vector2[length];
            baseNumberSize = new float[length];

            for (int j = 0; j < componets.Length; j++)
            {
                baseAnchoredPosition[j] = componets[j].rectTransform.anchoredPosition;
                baseNumberSize[j] = componets[j].diceNumberSize;
            }
        }
        public override void OnPlayableDestroy(Playable playable)
        {
            for (int j = 0; j < componets.Length; j++)
            {
                componets[j].rectTransform.anchoredPosition = baseAnchoredPosition[j];
                componets[j].diceNumberSize =  baseNumberSize[j];
            }
            base.OnPlayableDestroy(playable);
        }


        public override void PrepareFrame(Playable playable, FrameData info)
        {
            for (int i = 0; i < componets.Length; i++)
            {
                SetValue(playable, i, componets[i], baseAnchoredPosition[i], baseNumberSize[i]);
            }
        }

        public void SetValue(Playable playable, int diceParameterPtr, DiceValue component, Vector2 baseAnchoredPosition, float baseNumberSize)
        {
            int inputCount = playable.GetInputCount();
            double minTime = 100;
            bool noWeight = true;

            Vector2 randPositon = Vector2.zero;
            float diceNumberSize = 0;
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<DiceValueMultipleBehaviour>)playable.GetInput(i);
                DiceValueParameter diceProBehaviour = inputPlayable.GetBehaviour().diceParameters[diceParameterPtr];
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
