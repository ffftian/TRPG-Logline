using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;


public class HideTextMeshProBehaviour : PlayableBehaviour
{
    private TyperDialogTextMeshPro component;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (component == null)
        {
            component = playerData as TyperDialogTextMeshPro;
        }
        double percentage = 1 - (playable.GetTime() / playable.GetDuration());
        byte alpha = (byte)Math.Clamp(percentage * 255, 0, 255);
        component.SetAllAlpha(alpha);
    }
}

