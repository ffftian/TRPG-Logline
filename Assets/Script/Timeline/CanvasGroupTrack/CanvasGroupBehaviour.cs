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
    public class CanvasGroupBehaviour : PlayableBehaviour
    {
        public ProcessType processType;
        public float alpha;
        private float baseAlpha;
        private CanvasGroup component;
#if UNITY_EDITOR
        bool frist = true;
#endif

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            component = playerData as CanvasGroup;
#if UNITY_EDITOR
            if (frist)
            {
                frist = false;
                baseAlpha = component.alpha;
            }
#endif
            float Progress = (float)(playable.GetTime() / playable.GetDuration());


            switch (processType)
            {
                case ProcessType.淡入:
                    component.alpha = Progress;
                    break;
                case ProcessType.淡出:
                    component.alpha = 1 - Progress;
                    break;
                case ProcessType.维持:
                    component.alpha = alpha;
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
                    component.alpha = baseAlpha;
                }
            }
#endif
        }
    }

}