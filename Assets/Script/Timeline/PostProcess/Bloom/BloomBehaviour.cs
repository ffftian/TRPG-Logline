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
    public class BloomData
    {
        public float threshold = 0.9f;
        public float intensity = 0;
        [InspectorName("散射")]
        [Range(0,1)]
        public float scatter = 0.7f;
    }

    public class BloomBehaviour : PostProcessBehaviour<Bloom, BloomData>
    {
    

        public override void Assign(Bloom postProcessComponent)
        {
            postProcessComponent.intensity.value = data.intensity;
            postProcessComponent.threshold.value = data.threshold;
            postProcessComponent.scatter.value = data.scatter;
        }
        public override void Obtain(Bloom postProcessComponent)
        {
            data.intensity = postProcessComponent.intensity.value;
            data.threshold = postProcessComponent.threshold.value;
            data.scatter = postProcessComponent.scatter.value;
        }
        public override void Enable(Bloom postProcessComponent)
        {
            postProcessComponent.intensity.overrideState = true;
            postProcessComponent.threshold.overrideState = true;
            postProcessComponent.scatter.overrideState = true;
        }
        public override void Disable(Bloom postProcessComponent)
        {
            postProcessComponent.intensity.overrideState = false;
            postProcessComponent.threshold.overrideState = false;
            postProcessComponent.scatter.overrideState = false;
        }

        public override void Plus(BloomData bloomData, float weight)
        {
            data.intensity += bloomData.intensity * weight;
            data.threshold += bloomData.threshold * weight;
            data.scatter += bloomData.scatter * weight;
        }

        public override void Clear()
        {
            data.intensity = 0;
            data.threshold = 0;
            data.scatter = 0;
        }
    }
}
