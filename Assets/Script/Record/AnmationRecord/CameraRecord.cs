#if UNITY_EDITOR
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class CameraRecord : BaseAnmationRecord
{
    public Vector3 defaultPosition;
    public float defaultOrthgraphicSize;
    public float defaultFieldOfView;
    [ContextMenu("记录当前值为初始值")]
    protected override void RecordDefultValue()
    {
        defaultPosition = recordObjectTransform.localPosition;
        Camera camera = recordObjectTransform.GetComponent<Camera>();
        defaultOrthgraphicSize = camera.orthographicSize;
        defaultFieldOfView = camera.fieldOfView;
    }
    //protected override void RecordData(string assetName, string targetName, int serial, AnimationClip animationClip, EditorCurveBinding[] editorCurveBindings)
    //{
    //    if (targetName != recordObjectTransform.name) return;
    //    if (recordAnmationAsset == null)
    //    {
    //        recordAnmationAsset = RecordAnmationAsset.GetAsset(assetName, recordObjectTransform.name);
    //    }
    //    JObject curveValue = new JObject();
    //    foreach (EditorCurveBinding editorCurveBinding in editorCurveBindings)
    //    {
    //        AnimationCurve curveX = AnimationUtility.GetEditorCurve(animationClip, editorCurveBinding);
    //        Keyframe frame = curveX.keys[curveX.length - 1];
    //        string path = editorCurveBinding.path;
    //        if (!string.IsNullOrEmpty(path))
    //        {
    //            path += "/";
    //        }
    //        path += editorCurveBinding.propertyName;
    //        curveValue.Plus(path, frame.value);
    //        Debug.Log($"保存{path}成功值为{frame.value}序号为{serial}");
    //    }
    //    recordAnmationAsset.RecordValue(serial, curveValue.ToString());
    //    return;

    //    //foreach (EditorCurveBinding editorCurveBinding in editorCurveBindings)
    //    //{
    //    //    AnimationCurve curveX = AnimationUtility.GetEditorCurve(animationClip, editorCurveBinding);
    //    //    Keyframe frame = curveX.keys[curveX.length - 1];
    //    //    string path = editorCurveBinding.path;
    //    //    if (!string.IsNullOrEmpty(path))
    //    //    {
    //    //        path += "/";
    //    //    }
    //    //    path += editorCurveBinding.propertyName;
    //    //    curveValue.Plus(path, frame.value);
    //    //    Debug.Log($"保存{path}成功值为{frame.value}序号为{serial}");
    //    //}
    //    //recordAnmationAsset.RecordValue(serial, curveValue.ToString());
    //    //return;
    //}

    public override void RecoverData(string assetName, int currentIndex)
    {
        List<JObject> offsetList = RecordAnmationAsset(assetName).GetkeyValueJsonFrontToBack(currentIndex);

        Vector3 localPosition = defaultPosition;
        Vector3 localEulerAngles = Vector3.zero;//基于旋转
        float orthgraphicSize = defaultOrthgraphicSize;
        float fieldOfView = defaultFieldOfView;
        bool orthographic = true;

        //offsetList.Reverse();//颠倒，因为字典是先入的先循环，会被覆盖

        foreach (JObject offset in offsetList)
        {
            float? value;
            #region m_LocalPosition
            Vector3 cachePos = Vector3.zero;
            Quaternion cashQuaternion = Quaternion.identity;

            //Quaternion.FromToRotation = (, localEulerAngles);
            value = offset["m_LocalPosition.x"]?.ToObject<float>();
            if (value != null)
            {
                cachePos.x = (float)value;
                //localPosition.x += (float)value;
            }
            value = offset["m_LocalPosition.y"]?.ToObject<float>();
            if (value != null)
            {
                cachePos.y = (float)value;
                //localPosition.y += (float)value;
            }

            value = offset["m_LocalPosition.z"]?.ToObject<float>();
            if (value != null)
            {
                cachePos.z = (float)value;
               //localPosition.z += (float)value;
            }
            //虽然求的很狗屎，但真的这样还原旋转了。
            Vector3 realPos = Vector3.zero;
            if (offset["localEulerAnglesRaw.x"]==null)
            {
                Debug.Log("预存旋转为空，启用手动运算");
                //如果用三角函数一个面还好，三个面算了，用TBN矩阵。
                //var an =  Quaternion.LookRotation(localEulerAngles, Vector3.forward);
                //var show = an.eulerAngles;
                Quaternion rot = Quaternion.Euler(localEulerAngles);
                //设TBN矩阵吧
                Vector3 right = rot * Vector3.right;
                Vector3 up = rot * Vector3.up;
                Vector3 forward = rot * Vector3.forward;
                Matrix4x4 matrix4X4 = new Matrix4x4(right, up, forward, new Vector4(0, 0, 0, 1));
                realPos = matrix4X4.MultiplyPoint(cachePos);
                //Mathf.Sin()
            }
            else
            {
                realPos = cachePos;
            }
            localPosition += realPos;
            #endregion
            #region localEulerAnglesRaw


            value = offset["localEulerAnglesRaw.x"]?.ToObject<float>();
            if (value != null) localEulerAngles.x += (float)value;

            value = offset["localEulerAnglesRaw.y"]?.ToObject<float>();
            if (value != null) localEulerAngles.y += (float)value;

            value = offset["localEulerAnglesRaw.z"]?.ToObject<float>();
            if (value != null) localEulerAngles.z += (float)value;
            #endregion
            value = offset["orthographic size"]?.ToObject<float>();
            if (value != null) orthgraphicSize = (float)value;//对于其他值来说是预取
            value = offset["field of view"]?.ToObject<float>();
            if (value != null) fieldOfView = (float)value;

            bool? valueBool = offset["orthographic"]?.ToObject<bool>();
            if (valueBool != null)
                orthographic = (bool)valueBool;
        }
        recordObjectTransform.localPosition = localPosition;
        recordObjectTransform.localEulerAngles = localEulerAngles;
        Camera saveCamera = recordObjectTransform.GetComponent<Camera>();

        saveCamera.orthographicSize = orthgraphicSize;
        saveCamera.orthographic = orthographic;
        saveCamera.fieldOfView = fieldOfView;
    }
}
#endif
