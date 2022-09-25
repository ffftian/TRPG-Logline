using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

[Serializable]
public class DialogueControlClip : PlayableAsset, ITimelineClipAsset
{

    public override double duration
    {
        get
        {
            return template.TextLength / template._speed;
        }
    }

    public DialogueControlBehaviour template = new DialogueControlBehaviour();

    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<DialogueControlBehaviour>.Create(graph, template);
    }
}
