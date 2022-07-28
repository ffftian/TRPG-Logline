#if _TIMELINE
using UnityEngine.Playables;
    public class CustomPlayableBehaviourMixerExample : PlayableBehaviour
    {

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);

            var inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<CustomPlayableBehaviour>)playable.GetInput(i);
                CustomPlayableBehaviour behaviour = inputPlayable.GetBehaviour();
                // 访问behaviour的各种字段属性，并进行混合
            }
        }
    }
#endif