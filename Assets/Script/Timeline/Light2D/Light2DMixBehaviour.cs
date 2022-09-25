using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine;

namespace MiaoTween
{
    public class Light2DMixBehaviour : PlayableBehaviour
    {
        private Light2D component;

        public float baseIntensity;
        public float basePointLightInnerRadius;
        public float basePointLightOuterRadius;
        public float basePointLightInnerAngle;
        public float basePointLightOuterAngle;
        public Color baseColor;



        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (component == null)
            {
                component = playerData as Light2D;
                baseIntensity = component.intensity;
                basePointLightInnerRadius = component.pointLightInnerRadius;
                basePointLightOuterRadius = component.pointLightOuterRadius;
                basePointLightInnerAngle = component.pointLightInnerAngle;
                basePointLightOuterAngle = component.pointLightOuterAngle;
                baseColor = component.color;
            }
            int inputCount = playable.GetInputCount();
            float intensity = 0;
            float pointLightInnerRadius = 0;
            float pointLightOuterRadius = 0;
            float pointLightInnerAngle = 0;
            float pointLightOuterAngle = 0;
            Color color = Color.clear;

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<Light2DSpotBehaviour>)playable.GetInput(i);
                Light2DSpotBehaviour behaviour = inputPlayable.GetBehaviour();
                intensity += behaviour.intensity * inputWeight;
                pointLightInnerRadius += behaviour.pointLightInnerOuterRadius.x * inputWeight;
                pointLightOuterRadius += behaviour.pointLightInnerOuterRadius.y * inputWeight;
                pointLightInnerAngle += behaviour.pointLightInnerOuterAngle.x * inputWeight;
                pointLightOuterAngle += behaviour.pointLightInnerOuterAngle.y * inputWeight;
                color += behaviour.color * inputWeight;
            }
            component.intensity = intensity;
            component.pointLightInnerRadius = pointLightInnerRadius;
            component.pointLightOuterRadius = pointLightOuterRadius;
            component.pointLightInnerAngle = pointLightInnerAngle;
            component.pointLightOuterAngle = pointLightOuterAngle;
            component.color = color;

        }
        public override void OnPlayableDestroy(Playable playable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (component != null)
                {
                    component.intensity = baseIntensity;
                    component.pointLightInnerRadius = basePointLightInnerRadius;
                    component.pointLightOuterRadius = basePointLightOuterRadius;
                    component.pointLightInnerAngle = basePointLightInnerAngle;
                    component.pointLightOuterAngle = basePointLightOuterAngle;
                    component.color = baseColor;
                }
            }
#endif
        }
    }
}
