using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;

[Serializable]
public class DialogueControlClip : PlayableAsset, ITimelineClipAsset
{
    public DialogueControlBehaviour template = new DialogueControlBehaviour();

    public ClipCaps clipCaps => ClipCaps.All;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        return ScriptPlayable<DialogueControlBehaviour>.Create(graph, template);//创建置入一个Playable数据,最后一行还能置入顺序类型。
    }
}
