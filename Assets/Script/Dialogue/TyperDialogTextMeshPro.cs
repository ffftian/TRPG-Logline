using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

/// <summary>
/// 用于TextMeshPro的打字机效果组件。
/// </summary>
public class TyperDialogTextMeshPro : MonoBehaviour
{
    public TMP_Text Text;
    public int maxIndex
    {
        get
        {
            return Text.textInfo.characterCount;
        }
    }

    protected double lastPercentage = 0;
    protected double currentPercentage = 0;


    //protected double showingTime;


    public void SetText(string text)
    {
        this.Text.text = text;
        //this.showingTime = showingTime;
        //Text.ForceMeshUpdate();
        Text.ForceMeshUpdate();
        Text.maxVisibleCharacters = Text.textInfo.characterCount;
        Text.ForceMeshUpdate();
        lastPercentage = 0;
        currentPercentage = 0;
    }
    public void ClearAllAlpha()
    {
        //SetCharacterAlpha(1, 0);
        for (int i = 0; i < Text.textInfo.characterCount; i++)
        {
            SetCharacterAlpha(i, 0);
        }
        Text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }
    public void VisualAllAlpha()
    {
        //SetCharacterAlpha(2, 1);
        for (int i = 0; i < Text.textInfo.characterCount; i++)
        {
            SetCharacterAlpha(i, 255);
        }
        Text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    public void SetAllAlpha(byte alpha)
    {
        //SetCharacterAlpha(2, 1);
        for (int i = 0; i < Text.textInfo.characterCount; i++)
        {
            SetCharacterAlpha(i, alpha);
        }
        Text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    private void SetCharacterAlpha(int index, byte alpha)
    {
        var materialIndex = Text.textInfo.characterInfo[index].materialReferenceIndex;
        var vertexColors = Text.textInfo.meshInfo[materialIndex].colors32;
        var vertexIndex = Text.textInfo.characterInfo[index].vertexIndex;
        vertexColors[vertexIndex + 0].a = alpha;
        vertexColors[vertexIndex + 1].a = alpha;
        vertexColors[vertexIndex + 2].a = alpha;
        vertexColors[vertexIndex + 3].a = alpha;
    }

    public void RefreshShow(double percentage)
    {
        double firstIndex = lastPercentage * maxIndex;
        double endIndex = percentage * maxIndex;
        //Debug.Log($"firstIndex:{firstIndex}");
        //Debug.Log($"endIndex:{endIndex}");
        if (endIndex > firstIndex)
        {
            for (int i = (int)firstIndex; i < (int)endIndex; i++)
            {
                SetCharacterAlpha(i, 255);
            }
            ///将小数部分转为阿尔法部分。
            byte alpha = (byte)Math.Clamp((endIndex - (int)endIndex) * 255, 0, 255);
            //Debug.Log($"{Alpha}/{endIndex}");
            SetCharacterAlpha((int)endIndex, alpha);
        }
        else
        {
            for (int i = (int)endIndex; i < (int)firstIndex; i++)
            {
                SetCharacterAlpha(i, 0);
            }
            byte alpha = (byte)Math.Clamp((firstIndex - (int)firstIndex) * 255, 0, 255);
            SetCharacterAlpha((int)firstIndex, alpha);
        }
        lastPercentage = percentage;
        Text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    /// <summary>
    /// 传统逐字显示系统
    /// </summary>
    /// <param name="percentage"></param>
    //public void RefreshShow(double percentage)
    //{
    //    currentPercentage = percentage;
    //    Text.maxVisibleCharacters = (int)(maxIndex * percentage);
    //}
}
