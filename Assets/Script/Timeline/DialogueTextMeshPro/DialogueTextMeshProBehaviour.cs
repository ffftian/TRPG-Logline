using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// 对话Timeline相关
/// </summary>
[Serializable]
public class DialogueTextMeshProBehaviour : PlayableBehaviour
{
    [SerializeField][TextArea] public string dialogue = "对话内容";

    [Tooltip("轨道运行时百分比偏移值")]
    public float offset = 0;
    private TyperDialogTextMeshPro component;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (component == null)
        {
            component = playerData as TyperDialogTextMeshPro;
            component.SetText(dialogue);
            component.ClearAllAlpha();
        }
#if UNITY_EDITOR
        else if (component.Text.text != dialogue)
        {
            component.SetText(dialogue);
            component.ClearAllAlpha();
        }
#endif

        double percentage = playable.GetTime() / playable.GetDuration();
        component.RefreshShow(percentage + offset);
        base.ProcessFrame(playable, info, playerData);
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        if (component != null)
        {
            component.Text.text = "";
            component.Text.maxVisibleCharacters = 9999;
        }
        base.OnPlayableDestroy(playable);
    }
}

