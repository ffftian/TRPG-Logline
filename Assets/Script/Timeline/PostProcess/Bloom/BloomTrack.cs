using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

namespace PostProcess
{

    [TrackClipType(typeof(BloomClip))]
    [TrackBindingType(typeof(Volume))]
    public class BloomTrack : BaseMixTrack<BloomMixBehaviour>
    {

    }
}
