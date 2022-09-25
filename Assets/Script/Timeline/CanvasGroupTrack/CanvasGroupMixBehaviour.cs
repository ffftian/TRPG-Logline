using UnityEngine;
using UnityEngine.Playables;

namespace MiaoTween
{
    public class CanvasGroupMixBehaviour : PlayableBehaviour
    {
        private CanvasGroup component;
        private float baseAlpha;
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {

            if (component == null)
            {
                component = playerData as CanvasGroup;
                baseAlpha = component.alpha;
            }
            int inputCount = playable.GetInputCount();
            float mixAlpha = 0;
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<CanvasGroupBehaviour>)playable.GetInput(i);
                CanvasGroupBehaviour behaviour = inputPlayable.GetBehaviour();
                mixAlpha += behaviour.alpha * inputWeight;
            }
            component.alpha = mixAlpha;
        }
        public override void OnPlayableDestroy(Playable playable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (component != null)
                {
                    component.alpha = baseAlpha;
                }
            }
#endif
        }
    }
}
