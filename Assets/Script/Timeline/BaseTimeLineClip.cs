using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class BaseTimeLineClip<T> : PlayableAsset, ITimelineClipAsset where T : class, IPlayableBehaviour, new()
{
    [SerializeField] public T template;
    virtual public ClipCaps clipCaps => ClipCaps.None;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        
        return ScriptPlayable<T>.Create(graph, template);
    }
    

}

