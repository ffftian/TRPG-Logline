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
    /// 需要加依照当前坐标锁完全横轴或Z坐标不变这样
    /// </summary>
    public class CameraFocusClip : BaseTimeLineClip<CameraFocusBehaviour>
    {
        public override double duration => 0.8f;
        public ExposedReference<Transform> focus;
        //public Vector3 baseCameraPosition;
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            template.focusTransform = focus.Resolve(graph.GetResolver());
            //baseCameraPosition = owner.transform.position;
            return base.CreatePlayable(graph, owner);
        }
    }
}
