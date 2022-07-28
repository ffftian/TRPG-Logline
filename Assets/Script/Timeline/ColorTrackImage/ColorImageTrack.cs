using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Miao
{
    [TrackColor(0.2f, 0.2f, 0.2f)]
    [TrackBindingType(typeof(Image))]
    [TrackClipType(typeof(ColorImageClip))]
    public class ColorImageTrack : TrackAsset
    {
    }
}
