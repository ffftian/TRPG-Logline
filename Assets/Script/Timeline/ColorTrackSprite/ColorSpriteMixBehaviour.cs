using UnityEngine;
using UnityEngine.Playables;

namespace MiaoTween
{
    public class ColorSpriteMixBehaviour : PlayableBehaviour
    {
        private SpriteRenderer component;
        private Color baseColor;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {

            if (component == null)
            {
                component = playerData as SpriteRenderer;
                baseColor = component.color;
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
            component.color = mixColor;
        }
        public override void OnPlayableDestroy(Playable playable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (component != null)
                {
                    component.color = baseColor;
                }
            }
#endif
        }
    }
}
