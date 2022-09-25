using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace MiaoTween
{
    [Serializable]
    public class Light2DSpotBehaviour : PlayableBehaviour
    {
        public float intensity = 1;
        [MinMaxSlider(0, 20, true)]
        public Vector2 pointLightInnerOuterRadius = new Vector2(0, 5);
        [MinMaxSlider(0, 360, true)]
        public Vector2Int pointLightInnerOuterAngle = new Vector2Int(0, 360);
        public Color color;
    }
}
