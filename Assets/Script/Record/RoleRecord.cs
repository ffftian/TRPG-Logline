#if UNITY_EDITOR
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
/// <summary>
/// 记录单一角色最近一次的位置信息并在编辑器模式下一个动画前还原
/// </summary>
[RequireComponent(typeof(DialogComponent))]
public class RoleRecord : BaseAnmationRecord
{
    public Vector3 defaultPosition;
    public Vector3 defaultScale;
  

    protected override void OnValidate()
    {
        base.OnValidate();
    }
    //override protected void RecordData(string assetName, string targetName, int serial, AnimationClip animationClip, EditorCurveBinding[] editorCurveBindings)
    //{
    //    if (targetName != recordObjectTransform.name) return;
    //    if (recordAnmationAsset == null)
    //    {
    //        recordAnmationAsset = RecordAnmationAsset.GetAsset(assetName, recordObjectTransform.name);
    //    }
    //    //JTokenWriter jTokenWriter = new JTokenWriter();
    //    JObject curveValue = new JObject();
    //    //jTokenWriter.WriteStartObject();
    //    //这里是曲线应该是每条数据一类曲线
    //    foreach (EditorCurveBinding editorCurveBinding in editorCurveBindings)
    //    {
    //        //try
    //        //{
    //        AnimationCurve curve = AnimationUtility.GetEditorCurve(animationClip, editorCurveBinding);
    //        Keyframe frame = curve.keys[curve.length - 1];
    //        //editorCurveBinding.path 只有空path才证明为角色自己的修正路径
    //        string path = editorCurveBinding.path;//一般就是空
    //        if (!string.IsNullOrEmpty(path))
    //        {
    //            path += "/";
    //        }
    //        path += editorCurveBinding.propertyName;
    //        curveValue.Add(path, frame.value);
    //        //Debug.Log($"保存{targetName},{path}成功值为{frame.value}");
    //        //}
    //        //catch(Exception e)
    //        //{
    //        //    Debug.Log($"<color=red>{e}</color>");
    //        //}
    //    }
    //    recordAnmationAsset.RecordValue(serial, curveValue.ToString());
    //    //Debug.Log($"写入{targetName}成功");
    //    return;
    //}

    //改成右键菜单的形式，防止误点
    [ContextMenu("记录当前值为默认值")]
    //[Button("记录当前值为默认值")]
    override protected void RecordDefultValue()
    {
        defaultPosition = recordObjectTransform.localPosition;
        defaultScale = recordObjectTransform.localScale;
    }


    /// <summary>
    /// 归还当前对象最近的运动位置
    /// </summary>
    public override void RecoverData(string messageAssetName, int currentIndex)
    {
      
        List<JObject> offsetList = RecordAnmationAsset(messageAssetName).GetkeyValueJsonFrontToBack(currentIndex);

        Vector3 localPosition = defaultPosition;
        Vector3 localScale = defaultScale;

        foreach (JObject offset in offsetList)
        {
            float? value;
            value = offset["m_LocalPosition.x"]?.ToObject<float>();
            if (value != null) localPosition.x += (float)value;

            value = offset["m_LocalPosition.y"]?.ToObject<float>();
            if (value != null) localPosition.y += (float)value;

            value = offset["m_LocalPosition.z"]?.ToObject<float>();
            if (value != null) localPosition.z += (float)value;

            value = offset["m_LocalScale.x"]?.ToObject<float>();
            if (value != null) localScale.x = (float)value;

            value = offset["m_LocalScale.y"]?.ToObject<float>();
            if (value != null) localScale.y = (float)value;

            value = offset["m_LocalScale.z"]?.ToObject<float>();
            if (value != null) localScale.z = (float)value;
        }
        recordObjectTransform.localPosition = localPosition;
        recordObjectTransform.localScale = localScale;
        //Debug.Log($"已经重新复写{recordObject}的值");

    }
}
#endif
