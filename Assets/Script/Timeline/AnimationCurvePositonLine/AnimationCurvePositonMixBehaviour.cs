using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine;

namespace Miao
{
    public class AnimationCurvePositonMixBehaviour : PlayableBehaviour
    {
        private Transform component;
        private RectTransform rectCompoent;
        private Vector3 basePositon;
        private Vector2 anchoredBasePosition;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            if (component == null)
            {
                rectCompoent = playerData as RectTransform;
                if (rectCompoent != null)
                {
                    anchoredBasePosition = rectCompoent.anchoredPosition;
                }
                component = playerData as Transform;
                if (component == null) return;
                basePositon = component.position;
            }

            int inputCount = playable.GetInputCount();

            if (rectCompoent == null)
            {
                Vector3 postion = Vector3.zero;
                postion.z = basePositon.z;
                for (int i = 0; i < inputCount; i++)
                {
                    float inputWeight = playable.GetInputWeight(i);
                    var inputPlayable = (ScriptPlayable<AnimationCurvePositonBehaviour>)playable.GetInput(i);
                    AnimationCurvePositonBehaviour behaviour = inputPlayable.GetBehaviour();

                    postion.x += behaviour.curveX.Evaluate((float)inputPlayable.GetTime()) * inputWeight;
                    postion.y += behaviour.curveY.Evaluate((float)inputPlayable.GetTime()) * inputWeight;
                }
                component.position = basePositon + postion;
            }
            else
            {
                Vector2 anchoredPosition = Vector2.zero;
                for (int i = 0; i < inputCount; i++)
                {
                    float inputWeight = playable.GetInputWeight(i);
                    var inputPlayable = (ScriptPlayable<AnimationCurvePositonBehaviour>)playable.GetInput(i);
                    AnimationCurvePositonBehaviour behaviour = inputPlayable.GetBehaviour();

                    anchoredPosition.x += behaviour.curveX.Evaluate((float)inputPlayable.GetTime()) * inputWeight;
                    anchoredPosition.y += behaviour.curveY.Evaluate((float)inputPlayable.GetTime()) * inputWeight;
                }
                rectCompoent.anchoredPosition = anchoredBasePosition + anchoredPosition;
            }

        }
        public override void OnPlayableDestroy(Playable playable)
        {
            //#if UNITY_EDITOR
            //            if (!Application.isPlaying)
            //            {'
            if (rectCompoent != null)
            {
                rectCompoent.anchoredPosition = anchoredBasePosition;
            }
            else if (component != null)
            {
                component.position = basePositon;
            }
            //            }
            //#endif
        }
    }
}
