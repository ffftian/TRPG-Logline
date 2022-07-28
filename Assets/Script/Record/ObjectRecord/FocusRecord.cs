#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;

public class FocusRecord : BaseObjectRecord
{
    public DialogComponent dialogComponent;

    protected void Reset()
    {
        dialogComponent = GetComponent<DialogComponent>();
    }
    protected int MaxPtr
    {
        get
        {
            return dialogComponent.messageAsset.messageDataList.Count;
        }
    }

    protected RecordArrayAsset recordAsset;
    protected Vector3Converter converter = new Vector3Converter();
    public override void RecordData(string messageAssetName, string targetName, int ptr, object value)
    {
        if (targetName != recordObjectTransform.name) return;
        if (recordAsset == null)
        {
            recordAsset = RecordArrayAsset.GetAsset(nameof(FocusRecord) + messageAssetName, recordObjectTransform.name, MaxPtr);
        }
        Debug.Log($"保存{targetName}成功，值{value}");
        recordAsset.RecordValue(ptr, JsonConvert.SerializeObject(value, converter));
    }

    [Button("加载上一次的摄像机自动对焦")]
    public void LoadCameraLastFocus()
    {
        string value =  recordAsset.LastRecoverValue(dialogComponent.serialPtr);
        if (!string.IsNullOrEmpty(value))
        {
            recordObjectTransform.position = JsonConvert.DeserializeObject<Vector3>(value, converter);
        }
    }

    public override void RecoverData(string messageAssetName, int currentIndex)
    {
        recordAsset = RecordArrayAsset.GetAsset(nameof(FocusRecord) + messageAssetName, recordObjectTransform.name, MaxPtr);
        return;//不需要加入公有同步队列
    }
}
#endif
