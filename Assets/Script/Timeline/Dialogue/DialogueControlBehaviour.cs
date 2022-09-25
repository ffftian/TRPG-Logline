using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 对话Timeline相关
/// </summary>
[Serializable]
public class DialogueControlBehaviour : PlayableBehaviour
{
    [SerializeField][TextArea] public string dialogue = "对话内容";
    [SerializeField][Range(3f, 100f)] public double _speed = 10f;
    private TyperDialogue component;

    public int TextLength
    {
        get
        {
            return dialogue.Length;
        }
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (component == null)
        {
            component = playerData as TyperDialogue;
            component.SetText(dialogue);
        }
       

        component.SetIndex((int)(playable.GetTime() * _speed));
        component.UpdateText();
        //*ouble percentage = playable.GetTime() / playable.GetDuration();
        base.ProcessFrame(playable, info, playerData);
    }
}
