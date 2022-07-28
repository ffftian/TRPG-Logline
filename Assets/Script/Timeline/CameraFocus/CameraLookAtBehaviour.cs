using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;


public class CameraLookAtBehaviour : PlayableBehaviour
{
    private bool frist = true;
    private Camera component;
    [NonSerialized]
    public Transform focusTransform;//聚焦到的位置
    private float baseCameraSize;
    private Vector3 baseCameraPosition;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);
        if (frist)
        {
            if (playerData == null)
            {
                component = Camera.main;
            }
            else
            {
                component = playerData as Camera;
            }
            //Debug.Log("第一次进入");
            frist = false;
            baseCameraPosition = component.transform.position;
            baseCameraSize = component.orthographicSize;
        }
        component.transform.position = focusTransform.transform.position;
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (component != null)
            {
                component.transform.position = baseCameraPosition;
                component.orthographicSize = baseCameraSize;
            }
        }
#endif
    }

}
