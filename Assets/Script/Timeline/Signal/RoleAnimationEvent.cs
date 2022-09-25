using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class RoleAnimationEvent : MonoBehaviour
{
    [Title("用于事件监听，但只在播放时有效，避免修改编辑器数据导致bug出现")]
    private SkeletonAnimation skeletonAnimation;
    private MeshRenderer meshRenderer;
    private AudioSource audioSource;

    public void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        meshRenderer = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        skeletonAnimation.AnimationState.Event += AnimationState_Event;
    }

    private void AnimationState_Event(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        switch (e.Data.Name)
        {
            case "SortingLayer":
                meshRenderer.sortingLayerName = e.String;
                meshRenderer.sortingOrder = e.Int;
                break;
            case "Audio":
                AudioClip clip = Resources.Load<AudioClip>($"Audio/{e.String}");
                if (e.Float != 0)
                {
                    audioSource.PlayOneShot(clip, e.Float);
                }
                else
                {
                    audioSource.PlayOneShot(clip);
                }
                break;
        }
    }
}

