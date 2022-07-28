using Spine.Unity;
using Spine.Unity.Prototyping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Miao
{

    [Serializable]
    public class ColorSpineBehaviour : PlayableBehaviour
    {
        public ProcessType darkType;
        public Color color = Color.white;
        private SkeletonAnimation component;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            component = playerData as SkeletonAnimation;
            float Progress = (float)(playable.GetTime() / playable.GetDuration());


            switch (darkType)
            {
                case ProcessType.淡入:
                    component.skeleton.SetColor(new Color(color.r, color.g, color.b, Progress));
                    break;
                case ProcessType.淡出:
                    component.skeleton.SetColor(new Color(color.r, color.g, color.b, 1 - Progress));
                    break;
                case ProcessType.维持:
                    component.skeleton.SetColor(color);
                    break;

            }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
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

