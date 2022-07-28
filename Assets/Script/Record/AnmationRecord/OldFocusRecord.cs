#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
[Obsolete("由于值的交互过于复杂固弃用，因为无法有效确实获得摄像机在两次Focus的坐标")]
public class OldFocusRecord : BaseObjectRecord//虽然使用BaseObjectRecord，但可以用这个来存
{
    protected RecordAnmationAsset recordAnmationAsset;
    /// <summary>
    /// 这个仿Anmation来存
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="targetName"></param>
    /// <param name="serial"></param>
    /// <param name="value"></param>
    public override void RecordData(string assetName, string targetName, int serial, object valueT)
    {
        if (targetName != recordObjectTransform.name) return;
        if (recordAnmationAsset == null)
        {
            recordAnmationAsset = RecordAnmationAsset.GetAsset(assetName, recordObjectTransform.name);
        }
        //recordAnmationAsset.RecordValue(serial, value.ToString());
        JObject curveValue = new JObject();
        Vector3 value = (Vector3)valueT;
        curveValue.Add("m_LocalPosition.x", value.x);
        curveValue.Add("m_LocalPosition.y", value.y);
        curveValue.Add("m_LocalPosition.z", value.z);
        Debug.Log($"保存Focus成功，{targetName}成功值为{value}");

        recordAnmationAsset.RecordValue(serial, curveValue.ToString());
    }

    public override void RecoverData(string messageAssetName, int currentIndex)
    {
        return;//不需要计算，因为Camera有CameraRecord进行计算
        //recordAnmationAsset = RecordAnmationAsset.GetAsset(messageAssetName, recordObjectTransform.name);
        //List<JObject> offsetList = recordAnmationAsset.GetkeyValueJsonFrontToBack(currentIndex);
        //Vector3 localPosition = this.GetComponent<CameraRecord>().defaultPosition;
        //float orthgraphicSize = this.GetComponent<CameraRecord>().defaultOrthgraphicSize;
        ////offsetList.Reverse();//颠倒，因为字典是先入的先循环，会被覆盖

        //foreach (JObject offset in offsetList)
        //{
        //    float? value;
        //    value = offset["m_LocalPosition.x"]?.ToObject<float>();
        //    if (value != null) localPosition.x += (float)value;

        //    value = offset["m_LocalPosition.y"]?.ToObject<float>();
        //    if (value != null) localPosition.y += (float)value;

        //    value = offset["m_LocalPosition.z"]?.ToObject<float>();
        //    if (value != null) localPosition.z += (float)value;

        //    value = offset["orthographic size"]?.ToObject<float>();
        //    if (value != null) orthgraphicSize = (float)value;//对于其他值来说是预取
        //}
        //recordObjectTransform.localPosition = localPosition;
        //recordObjectTransform.GetComponent<Camera>().orthographicSize = orthgraphicSize;
    }
}

#endif