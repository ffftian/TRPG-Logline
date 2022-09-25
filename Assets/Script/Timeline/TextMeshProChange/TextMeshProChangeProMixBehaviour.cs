using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;


public class TextMeshProChangeMixBehaviour : PlayableBehaviour
{
    private TMP_Text component;
    private string baseText;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (component == null)
        {
            component = (playerData as TMP_Text);
            baseText = component.text;
        }
        base.ProcessFrame(playable, info, playerData);
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        if (!Application.isPlaying)
        {
            if (component != null)
            {
                component.SetText(baseText);
                component.maxVisibleCharacters = 10000;
            }
        }
    }
}

