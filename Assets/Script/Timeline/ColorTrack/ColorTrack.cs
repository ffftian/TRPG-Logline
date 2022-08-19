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
    [TrackBindingType(typeof(Color))]
    [TrackClipType(typeof(ColorBehaviour))]
    abstract public class ColorTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<ColorMixBehaviour>.Create(graph, inputCount);
        }
    }
}
