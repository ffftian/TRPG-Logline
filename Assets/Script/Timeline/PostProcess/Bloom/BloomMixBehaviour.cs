using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



namespace PostProcess
{
    //public class BloomMixBehaviour : PostProcessComponentMixBehaviour<PostProcessBehaviour<Bloom>, Bloom>
    //{

    //}

    public class BloomMixBehaviour : PostProcessComponentMixBehaviour<BloomBehaviour, BloomData, Bloom>
    {
        
    }
}
