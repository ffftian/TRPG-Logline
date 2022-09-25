using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PostProcess
{
    public abstract class PostProcessComponentMixBehaviour<TBehavour, TData, TComponent> : PlayableBehaviour where TBehavour : PostProcessBehaviour<TComponent, TData>, new() where TComponent : VolumeComponent, IPostProcessComponent where TData : new()
    {
        private Volume volume;
        private TBehavour baseBehaviour;
        private TComponent component;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (volume == null)
            {
                volume = (Volume)playerData;
                volume.profile.TryGet<TComponent>(out component);
                baseBehaviour = new TBehavour();
                baseBehaviour.data = new TData();
                baseBehaviour.Obtain(component);
                baseBehaviour.Enable(component);
            }
            int inputCount = playable.GetInputCount();
            TBehavour tempBehavour = new TBehavour();
            tempBehavour.data = new TData();
            tempBehavour.Clear();

            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<TBehavour>)playable.GetInput(i);
                TBehavour behaviour = inputPlayable.GetBehaviour();

                tempBehavour.Plus(behaviour.data, inputWeight);
            }
            tempBehavour.Assign(component);
        }
        public override void OnPlayableDestroy(Playable playable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (volume != null)
                {
                    baseBehaviour.Obtain(component);
                    baseBehaviour.Disable(component);
                }
            }
#endif
        }
    }
}

