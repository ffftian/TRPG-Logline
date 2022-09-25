using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine;

namespace Miao
{
    [Serializable]
    public class AnimationCurvePositonBehaviour : PlayableBehaviour
    {
        public AnimationCurve curveX = new AnimationCurve();
        public AnimationCurve curveY = new AnimationCurve();
    }
}
