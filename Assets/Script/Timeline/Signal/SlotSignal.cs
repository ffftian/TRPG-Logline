using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;


public class SlotSignal : MonoBehaviour
{
    [SpineSlot(dataField: "skeletonRenderer", includeNone: true)]
    public string slotName;
    SkeletonAnimation skeletonAnimation;
    private Slot IteamSlot;

    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        IteamSlot = skeletonAnimation.skeleton.FindSlot(slotName);
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
