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
    public class ColorSpineMixBehaviour : PlayableBehaviour
    {
        private SkeletonAnimation component;
        private Color baseColor;


        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {

            if (component == null)
            {
                component = playerData as SkeletonAnimation;
            }
            int inputCount = playable.GetInputCount();
            Color mixColor = Color.clear;
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<ColorBehaviour>)playable.GetInput(i);
                ColorBehaviour colorImageBehaviour = inputPlayable.GetBehaviour();
                mixColor += colorImageBehaviour.color * inputWeight;
            }
            component.skeleton.SetColor(mixColor);
        }
        public override void OnPlayableDestroy(Playable playable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (component != null)
                {
                    component.skeleton.SetColor(component.GetComponent<SkeletonColorInitialize>().skeletonColor);
                }
            }
#endif
        }
    }
}

