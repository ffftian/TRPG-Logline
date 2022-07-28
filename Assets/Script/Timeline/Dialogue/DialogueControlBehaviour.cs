using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 对话Timeline相关
/// </summary>
[Serializable]
public class DialogueControlBehaviour : CustomPlayableBehaviour
{
    [SerializeField] [TextArea] public string dialogue = "对话内容";
    [SerializeField] [Range(3f, 100f)] public float _speed = 10f;

    private TyperDialogue component;
#if UNITY_EDITOR
    private bool isInited = false;
#endif

    protected override void Init(object playerData)
    {
        if(playerData==null)
        {
            Debug.Log("Dialogue为空，请重新设置");
            return;
        }

        component = playerData as TyperDialogue;
        component.SetText(dialogue);
#if UNITY_EDITOR
        UpdateText();
#endif
    }
    protected override void OnStay()
    {
#if UNITY_EDITOR
        if (Application.isPlaying || isInited) UpdateText();
#else
        UpdateText();
#endif
    }

    private void UpdateText()
    {
        int index = Mathf.FloorToInt((float)Time * _speed);
        component.SetIndex(index);
        component.UpdateText();
    }

}
