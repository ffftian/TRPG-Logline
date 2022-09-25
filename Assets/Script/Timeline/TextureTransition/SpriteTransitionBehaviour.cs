using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


namespace Miao
{
    //[Serializable]
    public class SpriteTransitionBehaviour : PlayableBehaviour
    {
        const string bar = "bar";
        protected Sprite originalSprite;
        public Sprite transitionSprite;//外部赋值
        public SpriteRenderer component;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (component == null)
            {
                component = playerData as SpriteRenderer;
                originalSprite = component.sprite;
            }
            float Progress = (float)(playable.GetTime() / playable.GetDuration());//从0到1


            if (Progress < 0.5f)
            {
                if (component.sprite != originalSprite)
                {
                    component.sprite = originalSprite;
                }
                //Debug.Log(1 - (Progress * 2));
                component.material.SetFloat(bar, 1 - (Progress * 2));
            }
            else
            {
                if (component.sprite != transitionSprite)
                {
                    component.sprite = transitionSprite;
                }
                component.material.SetFloat(bar, Mathf.Abs((0.5f - Progress) * 2));
            }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (component != null)
                {
                    component.sprite = originalSprite;
                    component.material.SetFloat(bar,1);
                }
            }
#endif
        }
    }
}
