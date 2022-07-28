using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Experimental
{
    /// <summary>
    /// 按照预定速度把眼睛看向目标,用于Timeline时本身的时间轴不提供太多意义。
    /// </summary>
    public class MiaoEyeLook : MonoBehaviour
    {

        public Transform target;//目标所在位置。
        public float radius;//最大范围
        public float Progress;
        //上一个角色所在坐标。
        public Vector3 lastTargetPos;


        protected SkeletonUtility hierarchy;
        protected Transform eyes;

        //眼睛最初的坐标位置。
        public Vector3 originPos;

        private void OnValidate()
        {
            hierarchy = GetComponent<SkeletonUtility>();
            eyes = hierarchy.boneRoot.GetChild(0).Find("eyes");

        }
        public void ResetPos()
        {
            eyes.localPosition = originPos;
        }

        /// <summary>
        /// 应该直接不考虑update;
        /// </summary>
        public void SeeTarget()
        {
            if (target == null) { return; }

            Vector3 globalPos = transform.position + originPos;
            //计算视野到底的插值。
            Vector3 EndPos = globalPos + ((target.transform.position - globalPos) * radius);

            //Debug.Log((EndPos - eyes.position).sqrMagnitude);
            if ((EndPos - eyes.position).sqrMagnitude < 0.001f)
            {
                lastTargetPos = EndPos;
            }
            if (lastTargetPos != Vector3.zero)
            {
                Vector3 当前位置 = lastTargetPos;
                Vector3 看向的位置 = globalPos + ((target.transform.position - globalPos) * radius);
                eyes.position = Vector3.Lerp(当前位置, 看向的位置, Progress);
            }
            else
            {
                eyes.position = Vector3.Lerp(globalPos, target.transform.position, Progress * radius);
            }
        }
    }
}





