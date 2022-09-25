using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

[Serializable]
public class DialogueTextMeshProClip : PlayableAsset, ITimelineClipAsset
{
    public DialogueTextMeshProBehaviour template = new DialogueTextMeshProBehaviour();

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<DialogueTextMeshProBehaviour>.Create(graph, template);
    }
}

