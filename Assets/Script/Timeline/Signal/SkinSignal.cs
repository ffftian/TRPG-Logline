using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.Playables;

//看MixAndMatchSkinsExample
public class SkinSignal : MonoBehaviour//, INotificationReceiver
{
    [SpineSkin] public string choiceClothes;
    [SpineSkin] public string choiceClothes2;

    public SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState animationState;
    SkeletonData skeletonData;

    public Skin nakedCharacterSkin;//角色裸体状态皮肤
    public Skin ClothesSkin;//只有衣服
    public Skin ClothesSkin2;

    public Skin RoleHaveClothesSkin;//角色当前着装的皮肤
    [Tooltip("手电筒的光")]
    public UnityEngine.Rendering.Universal.Light2D Flashlight;


    /////<摘要>
    /////在发送通知时调用。示例SignalReceiver
    /////</summary> 
    /////<param name=“origin”>发送通知的可播放文件。</param>
    /////<param name=“notification”>收到的通知。只处理<see cref=“SignalEmitter”/>类型的通知。</param>
    /////<param name=“context”>取决于通知类型的用户定义数据。使用它来传递必要的信息，这些信息会随着每次调用而更改。</param>
    //public void OnNotify(Playable origin, INotification notification, object context)
    //{
    //}

    protected void Start()
    {
        skeletonData = skeletonAnimation.skeleton.Data;
        animationState = skeletonAnimation.state;
        //FusionSkin();
        //额外，进入游戏就调用穿衣服
        设置衣物();
    }

    protected void FusionSkin()
    {
        RoleHaveClothesSkin = new Skin("上衣人体");
        //skeletonData = skeletonAnimation.skeleton.Data;
        nakedCharacterSkin = skeletonData.FindSkin(skeletonAnimation.initialSkinName);
        ClothesSkin = skeletonData.FindSkin(choiceClothes);
        ClothesSkin2 = skeletonData.FindSkin(choiceClothes2);
        RoleHaveClothesSkin.AddSkin(nakedCharacterSkin);
        RoleHaveClothesSkin.AddSkin(ClothesSkin);
        if (ClothesSkin2 != null)
        {
            RoleHaveClothesSkin.AddSkin(ClothesSkin2);
        }
    }
    /// <summary>
    /// 设置层级
    /// </summary>
    /// <param name="sortingLayerName"></param>
    public void SetSortingLayer(string sortingLayerName)
    {
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = sortingLayerName;
    }
    public void ClearTrack(int TrackIndex)
    {
        skeletonAnimation.AnimationState.ClearTrack(TrackIndex);

    }

    /// <summary>
    /// 设置层级
    /// </summary>
    /// <param name="OrderLayer"></param>
    public void SetOrderLayer(int OrderLayer)
    {
        MeshRenderer meshRenderer = this.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = OrderLayer;
    }

    /// <summary>
    /// 在演出途中设置新的皮肤情况
    /// </summary>
    /// <param name="SkinName"></param>
    public void AddNewSkin(string SkinName)
    {
        Skin newSkin = skeletonData.FindSkin(SkinName);
        RoleHaveClothesSkin.AddSkin(newSkin);


        skeletonAnimation.skeleton.SetSkin(RoleHaveClothesSkin);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();
    }

    public void RemoveSkin(string SkinName)
    {
        Skin skin = skeletonData.FindSkin(SkinName);
        foreach (Skin.SkinEntry data in skin.Attachments)
        {
            RoleHaveClothesSkin.RemoveAttachment(data.SlotIndex, data.Name);
        }
        skeletonAnimation.skeleton.SetSkin(RoleHaveClothesSkin);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();
    }


    #region 特殊动画回调
    public void 启用手电筒()
    {
        animationState.Event -= FlashlightClose;
        animationState.Event += FlashlightOpen;
    }
    public void 禁用手电筒()
    {
        animationState.Event -= FlashlightOpen;
        animationState.Event += FlashlightClose;
    }

    //用Signal加在事件前即可，手电筒会自动检索当前动画的轨道并置入，原理是使用的Anmation事件。
    private void FlashlightOpen(TrackEntry trackEntry, Spine.Event e)
    {
        switch(e.String)
        {
            case "ItemShow":
                AddNewSkin("Weapon/手电筒");
                break;
            case "ItemUse":
                Flashlight.gameObject.SetActive(true);
                break;
        }
    }
    //关闭手电筒会自动检索当前动画的轨道并置入，原理是使用的事件自动播放
    private void FlashlightClose(TrackEntry trackEntry, Spine.Event e)
    {
        switch (e.String)
        {
            case "ItemClose":
                RemoveSkin("Weapon/手电筒");
                break;
            case "ItemUse":
                Flashlight.gameObject.SetActive(false);
                break;
        }
    }
    #endregion




    /// <summary>
    /// 这个在Editor有冲突，不能在Editor模式下直接设置多重Skin，必须要进入Game模式。
    /// </summary>
    [Button]
    public void 设置衣物()
    {
        if (RoleHaveClothesSkin == null)
        {
            FusionSkin();
        }
        Debug.Log("调用穿衣服");
        skeletonAnimation.skeleton.SetSkin(RoleHaveClothesSkin);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();
        //skeletonAnimation.Initialize(true);
    }
    [Button]
    public void 脱下衣物()
    {
        Debug.Log("调用脱衣服");
        skeletonAnimation.skeleton.SetSkin(nakedCharacterSkin);
        skeletonAnimation.skeleton.SetSlotsToSetupPose();
    }
}

