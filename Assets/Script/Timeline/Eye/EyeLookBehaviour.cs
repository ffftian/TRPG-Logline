using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine;
namespace Experimental
{
    [Serializable]
    public class EyeLookBehaviour : PlayableBehaviour
    {
        public Transform target;
        public float radius = 0.05f;
        private MiaoEyeLook component;


        //注意:这个函数在运行时和编辑时被调用。在设置属性值时要记住这一点。 
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            //Debug.Log("Frame");
            base.ProcessFrame(playable, info, playerData);
            component = playerData as MiaoEyeLook;
            component.radius = radius;
            component.target = target;
            float Progress = (float)(playable.GetTime() / playable.GetDuration());
            component.Progress = Progress;
            component.SeeTarget();
        }
        /// <summary>
        /// 处于编辑器下并且删除脚本时，自动还原坐标。
        /// </summary>
        /// <param name="playable"></param>
        public override void OnPlayableDestroy(Playable playable)
        {
            if (component == null)
            {
                return;
            }

            //当处于编辑模式下时，关闭时还原成默认视角。
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                component.Progress = 0;
                component.lastTargetPos = Vector3.zero;
                component.ResetPos();
            }
#endif
            base.OnPlayableDestroy(playable);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="playable"></param>
        /// <param name="info"></param>
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            base.OnBehaviourPlay(playable, info);
        }


        public override void OnGraphStop(Playable playable)
        {
            base.OnGraphStop(playable);
        }
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
        }



    }
}