using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CameraLookAtClip : BaseTimeLineClip<CameraLookAtBehaviour>
{
    public override double duration => 1f;
    public ExposedReference<Transform> focus;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        template.focusTransform = focus.Resolve(graph.GetResolver());
        //baseCameraPosition = owner.transform.position;
        return base.CreatePlayable(graph, owner);
    }
}
