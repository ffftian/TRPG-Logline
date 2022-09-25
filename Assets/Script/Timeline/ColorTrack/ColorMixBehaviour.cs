using UnityEngine;
using UnityEngine.Playables;

namespace MiaoTween
{
    public class ColorMixBehaviour : PlayableBehaviour
    {
        public virtual Color color { get; set; }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
        }


    }
}

