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
    public class FilmGrainData
    {
        public FilmGrainLookup filmGrainLookup;
        [Range(0, 1)]
        public float intensity;
        [Range(0, 1)]
        public float response = 0.8f;
    }


    public class FilmGrainBehaviour : PostProcessBehaviour<FilmGrain, FilmGrainData>
    {
        public override void Assign(FilmGrain postProcessComponent)
        {
            postProcessComponent.type.value = data.filmGrainLookup;
            postProcessComponent.intensity.value = data.intensity;
            postProcessComponent.response.value = data.response;
        }
        public override void Obtain(FilmGrain postProcessComponent)
        {
            data.filmGrainLookup = postProcessComponent.type.value;
            data.intensity = postProcessComponent.intensity.value;
            data.response = postProcessComponent.response.value;
        }
        public override void Enable(FilmGrain postProcessComponent)
        {
            postProcessComponent.type.overrideState = true;
            postProcessComponent.intensity.overrideState = true;
            postProcessComponent.response.overrideState = true;
        }
        public override void Disable(FilmGrain postProcessComponent)
        {
            postProcessComponent.type.overrideState = false;
            postProcessComponent.intensity.overrideState = false;
            postProcessComponent.response.overrideState = false;
        }
        public override void Plus(FilmGrainData addData, float weight)
        {
            data.intensity += addData.intensity * weight;
            data.response += addData.response * weight;
            data.filmGrainLookup = addData.filmGrainLookup;
        }

        public override void Clear()
        {
            data.intensity = 0;
            data.response = 0;
        }

    }
}
