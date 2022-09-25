using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class TrackSignal : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    /// <summary>
    /// 设置层级
    /// </summary>
    /// <param name="sortingLayerName"></param>
    public void SetSortingLayer(string sortingLayerName)
    {
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = sortingLayerName;
    }
    public void ClearAllTrack()
    {
        skeletonAnimation.AnimationState.ClearTracks();
    }

    public void ClearTrack(int TrackIndex)
    {
        skeletonAnimation.AnimationState.ClearTrack(TrackIndex);
    }

    public void SetSkins(string skinNames)
    {
        string[] skinNamess = skinNames.Split(',');

        //Skin nick = new Skin("hebing");
        Skin skin = skeletonAnimation.skeleton.Data.FindSkin(skinNamess[0]);
        for (int i = 1; i < skinNamess.Length; i++)
        {
            skin.AddSkin(skeletonAnimation.skeleton.Data.FindSkin(skinNamess[i]));
        }
        skeletonAnimation.skeleton.SetSkin(skin);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();
    }


    public void SetSkin(string skinName)
    {
        Skin skin = skeletonAnimation.skeleton.Data.FindSkin(skinName);
        skeletonAnimation.skeleton.SetSkin(skin);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();
    }
}

