using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

public class HideTextMeshProClip : PlayableAsset, ITimelineClipAsset
{
    public HideTextMeshProBehaviour template = new HideTextMeshProBehaviour();

    public ClipCaps clipCaps => ClipCaps.None;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<HideTextMeshProBehaviour>.Create(graph, template);
    }
}

