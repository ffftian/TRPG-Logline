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
    public class ColorSpriteBehaviour : PlayableBehaviour
    {
        public ProcessType darkType;
        private Color baseColor;
        public Color color = Color.white;
        private SpriteRenderer component;
#if UNITY_EDITOR
        bool frist = true;
#endif

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            component = playerData as SpriteRenderer;
#if UNITY_EDITOR
            if (frist)
            {
                frist = false;
                baseColor = component.color;
            }
#endif
            float Progress = (float)(playable.GetTime() / playable.GetDuration());




            switch (darkType)
            {
                case ProcessType.淡入:
                    component.color = (new Color(color.r, color.g, color.b, Progress));
                    break;
                case ProcessType.淡出:
                    component.color = (new Color(color.r, color.g, color.b, 1 - Progress));
                    break;
                case ProcessType.维持:
                    component.color = color;
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
                    component.color = baseColor;
                }
            }
#endif
        }
    }
}

