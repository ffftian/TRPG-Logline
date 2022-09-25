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
    [TrackBindingType(typeof(TextNumberScrollBar))]
    [TrackClipType(typeof(TextNumberScrollBarClip))]
    public class TextNumberScrollBarTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<TextNumberScrollBarMixBehaviour>.Create(graph, inputCount);
        }
    }
}

