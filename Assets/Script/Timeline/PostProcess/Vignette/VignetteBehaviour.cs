using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

namespace PostProcess
{
    [Serializable]
    public class VignetteData
    {
        public Color color = Color.black;
        [Range(0f, 1f)]
        public float intensity;
        [Range(0f, 1f)]
        public float smoothness = 0.2f;
        public Vector2 center = new Vector2(0.5f, 0.5f);
        public bool rounded = false;
    }



    public class VignetteBehaviour : PostProcessBehaviour<Vignette, VignetteData>
    {

        public override void Assign(Vignette vignette)
        {
            vignette.color.value = data.color;
            vignette.intensity.value = data.intensity;
            vignette.smoothness.value = data.smoothness;
            vignette.center.value = data.center;
            vignette.rounded.value = data.rounded;


        }
        public override void Obtain(Vignette vignette)
        {
            data.color = vignette.color.value;
            data.smoothness = vignette.smoothness.value;
            data.center = vignette.center.value;
            data.rounded = vignette.rounded.value;
        }
        public override void Enable(Vignette vignette)
        {
            vignette.color.overrideState = true;
            vignette.intensity.overrideState = true;
            vignette.smoothness.overrideState = true;
            vignette.center.overrideState = true;
        }
        public override void Disable(Vignette vignette)
        {
            vignette.color.overrideState = false;
            vignette.intensity.overrideState = false;
            vignette.smoothness.overrideState = false;
            vignette.center.overrideState = false;
        }

        public override void Plus(VignetteData addData, float weight)
        {
            data.color += addData.color * weight;
            data.intensity += addData.intensity;
            data.smoothness += addData.smoothness;
            data.center += addData.center;
            data.rounded = addData.rounded;
        }
        public override void Clear()
        {
            data.color = Color.clear;
            data.intensity = 0;
            data.smoothness = 0;
            data.center = Vector2.zero;
            data.rounded = false;
        }
    }
}
