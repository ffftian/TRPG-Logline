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
    public class MovieScaleLineMixBehaviour : PlayableBehaviour
    {

        public RectTransform topBorder;
        public RectTransform bottomBorder;

        public Vector2 baseTop;
        public Vector2 baseBottom;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            base.PrepareFrame(playable, info);
            //if (volume == null)
            //{
            //    //baseTop = volume.topBorder.sizeDelta.y;
            //    //baseBottom = volume.bottomBorder.sizeDelta.y;
            //}
            int inputCount = playable.GetInputCount();
            Vector2 mixTop = Vector2.zero;
            Vector2 mixBotton = Vector2.zero;
            for (int i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<MovieScaleLineBehaviour>)playable.GetInput(i);
                MovieScaleLineBehaviour behaviour = inputPlayable.GetBehaviour();
                mixTop += inputWeight * behaviour.top;
                mixBotton += inputWeight * behaviour.bottom;
            }
            topBorder.sizeDelta = mixTop;
            bottomBorder.sizeDelta = mixBotton;
        }


        public override void OnPlayableDestroy(Playable playable)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (topBorder != null)
                {
                    topBorder.sizeDelta = baseTop;
                }
                if(bottomBorder!=null)
                {
                    bottomBorder.sizeDelta = baseBottom;
                }
            }
#endif
        }

    }


    //public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    //{
    //}

    
}
