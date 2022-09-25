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
    //[TrackBindingType(typeof(MovieScaleLine))]
    [TrackClipType(typeof(MovieScaleLineClip))]
    public class MovieScaleLineTrack : TrackAsset
    {
        public ExposedReference<RectTransform> topBorderReference;
        public ExposedReference<RectTransform> bottomBorderReference;
        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            ScriptPlayable<MovieScaleLineBehaviour> scriptPlayable =  (ScriptPlayable<MovieScaleLineBehaviour>)base.CreatePlayable(graph, gameObject, clip);
            MovieScaleLineBehaviour movieScaleLineBehaviour = scriptPlayable.GetBehaviour();
            return scriptPlayable;
        }
        
        protected override void OnCreateClip(TimelineClip clip)
        {
            //base.OnCreateClip(clip);
            //var movieScaleLineClip = (MovieScaleLineClip)clip.asset;
        }
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<MovieScaleLineMixBehaviour> scriptPlayable = ScriptPlayable<MovieScaleLineMixBehaviour>.Create(graph, inputCount);
            MovieScaleLineMixBehaviour movieScaleLineMixBehaviour = scriptPlayable.GetBehaviour();

            movieScaleLineMixBehaviour.topBorder = topBorderReference.Resolve(graph.GetResolver());
            movieScaleLineMixBehaviour.bottomBorder = bottomBorderReference.Resolve(graph.GetResolver());
            movieScaleLineMixBehaviour.baseTop = movieScaleLineMixBehaviour.topBorder.sizeDelta;
            movieScaleLineMixBehaviour.baseBottom = movieScaleLineMixBehaviour.bottomBorder.sizeDelta;

            return scriptPlayable;
        }


    }
}
