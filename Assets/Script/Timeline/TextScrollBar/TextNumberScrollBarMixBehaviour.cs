using Spine.Unity;
using Spine.Unity.Prototyping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

namespace Miao
{
    public class TextNumberScrollBarMixBehaviour : PlayableBehaviour
    {
        public TextNumberScrollBar component;
        bool baseActive;

        public override void OnPlayableDestroy(Playable playable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (component != null)
                {
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
                component = playerData as TextNumberScrollBar;
                component.Init();
                baseActive = component.gameObject.activeSelf;
            }
            int inputCount = playable.GetInputCount();
            int startIndex = -1;
            int endIndex = component.numberInterval;

            double maxTime = -100;
            double minTime = 100;
            bool noWeight = true;
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                if (inputWeight > 0)
                {
                    var inputPlayable = (ScriptPlayable<TextNumberScrollBarBehaviour>)playable.GetInput(i);
                    TextNumberScrollBarBehaviour behaviour = inputPlayable.GetBehaviour();
                    //var d=  inputPlayable.GetDuration();

                    double time = inputPlayable.GetTime();
                    //Debug.Log(time);
                    if (time > maxTime)
                    {
                        startIndex = behaviour.Number;
                        component.percentage = 1 - inputWeight;
                        maxTime = time;
                    }
                    if (time < minTime)
                    {
                        endIndex = behaviour.Number;
                        minTime = time;
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

            //Debug.Log(volume.percentage);
            //volume.percentage = fristWeight;
            component.SetMoveInterval(startIndex, endIndex);
            component.MoveTo();

            base.ProcessFrame(playable, info, playerData);
        }
    }
}
