using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

namespace Miao
{
    [TrackColor(0.2f, 0.2f, 0.2f)]
    [TrackBindingType(typeof(Image))]
    [TrackClipType(typeof(ColorClip))]
    public class ColorImageTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<ColorImageMixBehaviour>.Create(graph, inputCount);
        }
    }
}
