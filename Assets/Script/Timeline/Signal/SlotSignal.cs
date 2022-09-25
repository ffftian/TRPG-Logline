using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 特定骨骼绑定发送信号
/// </summary>
[DrawWithUnity]
public class SlotSignal : MonoBehaviour
{
    [SpineSlot(dataField: "skeletonRenderer", includeNone: true)]
    public string slotName;
    private SkeletonAnimation skeletonAnimation;
    private Slot IteamSlot;

    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        IteamSlot = skeletonAnimation.skeleton.FindSlot(slotName);
    }
    public void ShowAttachment(string itmeName)
    {
        IteamSlot.Data.AttachmentName = itmeName;
        IteamSlot.SetToSetupPose();
        //IteamSlot.Bone.SetToSetupPose();
        //skeletonAnimation.skeleton.SetToSetupPose();
    }

    public void ShowItem(string itmeName)
    {
        
        IteamSlot.Data.AttachmentName = $"Item/{itmeName}";
        skeletonAnimation.skeleton.SetToSetupPose();
        //IteamSlot.SetAttachmentToSetupPose();
    }

    public void ShowWeapon(string itmeName)
    {
        IteamSlot.Data.AttachmentName = $"Weapon/{itmeName}";
        skeletonAnimation.skeleton.SetToSetupPose();
        //IteamSlot.SetAttachmentToSetupPose();
    }

    public void HideItem()
    {
        IteamSlot.Data.AttachmentName = "";
        skeletonAnimation.skeleton.SetToSetupPose();
        //IteamSlot.SetAttachmentToSetupPose();
    }
}
