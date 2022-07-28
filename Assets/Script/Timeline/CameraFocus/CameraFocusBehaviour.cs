using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;


namespace Miao
{
    /// <summary>
    /// 看向特定角色！
    /// </summary>
    [Serializable]
    public class CameraFocusBehaviour : PlayableBehaviour
    {
        private bool frist = true;
        [NonSerialized]
        public Transform focusTransform;//聚焦到的位置
        private Camera component;
        public float cameraMoveSizeScale = 1.2f;//摄像机移动聚焦时的尺寸
        public bool LockX = false;//锁住X
        public bool LockY = true;//锁住Y
        //[NonSerialized]
        private float baseCameraSize;
        //[NonSerialized]
        private Vector3 baseCameraPosition;


        public override void OnPlayableCreate(Playable playable)
        {
            base.OnPlayableCreate(playable);
            if (component == null)
            {
                component = Camera.main;
            }
            SetCameraSize();
            //Debug.Log("摄像机位置赋值成功" + baseCameraPosition);
            //UnityEditor.EditorUtility.SetDirty(playable.);
        }
        public void SetCameraSize()
        {
            baseCameraPosition = component.transform.position;
            if (component.orthographic)
            {
                baseCameraSize = component.orthographicSize;
            }
            else
            {
                baseCameraSize = component.fieldOfView;
            }
        }


        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            //Debug.Log(baseCameraPosition);
            base.ProcessFrame(playable, info, playerData);
            //#if UNITY_EDITOR
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
                SetCameraSize();
                //Debug.Log("摄像机位置赋值成功" + baseCameraPosition);
            }
            //#endif

            Vector3 focusPosition = new Vector3(0, 0, baseCameraPosition.z);
            focusPosition.x = LockX ? baseCameraPosition.x : focusTransform.position.x;
            focusPosition.y = LockY ? baseCameraPosition.y : focusTransform.position.y;
            if (baseCameraPosition.x == focusPosition.x && baseCameraPosition.y == focusPosition.y)
            {
                Debug.Log("两者位置相同，不执行");
                return;
            }
            float progress = (float)(playable.GetTime() / playable.GetDuration());
            if (progress + Time.deltaTime > 1)//倍速时还原不正确的解决方案
            {
                progress = 1;
            }

            component.transform.position = Vector3.Lerp(baseCameraPosition, focusPosition, progress);
            float LStart = Mathf.Lerp(baseCameraSize, baseCameraSize * cameraMoveSizeScale, progress);
            float LEnd = Mathf.Lerp(baseCameraSize * cameraMoveSizeScale, baseCameraSize, progress);

            if (component.orthographic)
            {
                component.orthographicSize = Mathf.Lerp(LStart, LEnd, progress);//二阶贝塞尔曲线做缩放看向
            }
            else
            {
                component.fieldOfView = Mathf.Lerp(LStart, LEnd, progress);
            }


        }



        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
#if UNITY_EDITOR
            if (!Application.isPlaying)//如果未启动时则结束播放进行还原
            {
                if (component != null)
                {
                    component.transform.position = baseCameraPosition;
                    if (component.orthographic)
                    {
                        component.orthographicSize = baseCameraSize;
                    }
                    else
                    {
                        component.fieldOfView = baseCameraSize;
                    }
                }
            }

#endif
        }
    }

}