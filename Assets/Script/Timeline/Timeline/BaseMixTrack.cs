using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public abstract class BaseMixTrack<T> : TrackAsset where T : PlayableBehaviour, new()
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<T>.Create(graph, inputCount);
    }
}

