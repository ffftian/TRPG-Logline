//#if UNITY_EDITOR
using Spine.Unity;
using Spine.Unity.Playables;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

namespace Miao
{

    /// <summary>
    /// QQTimeLine骨骼生成器，不采用静态的原因是有一些前期设置。
    /// </summary>
    public class QQTimelineCreater
    {
        //当前保存资源文件夹地址名称
        public string saveDirectory;
        public string SpineDirectiory;
        public string DialogueDirectiory;

        /// <summary>
        /// 生成中，角色嘴型将反复用到，缓存读取到的资源将有效优化生成速度。
        /// </summary>
        public Dictionary<string, LipTrackExtension.MouthGroup[]> CacheMouthGroups = new Dictionary<string, LipTrackExtension.MouthGroup[]>();
        public QQTimelineCreater(string saveTimelineDirectory, string SpineDirectiory = @"Assets\AssetSpine", string DialogueDirectiory = @"Assets\AssetSpeak")
        {
            this.saveDirectory = saveTimelineDirectory;
            this.SpineDirectiory = SpineDirectiory;
            this.DialogueDirectiory = DialogueDirectiory;
            if (!Directory.Exists(saveTimelineDirectory))
            {
                Directory.CreateDirectory(saveTimelineDirectory);
            }

        }
        /// <summary>
        /// 命名规则：至少要拥有一个名为Idle的默认动画，嘴型只从小到大匹配，通过MouthO{数字},MouthC{数字}来实施。
        /// 数字可以跳着命名，越大的数字越阈值高。
        /// </summary>
        /// <param name="timelineName"></param>
        /// <param name="SpineAssetName=骨骼动画所在名称"></param>
        /// <param name="SpeakAssetName=嘴型资源所在名称"></param>
        /// <param name="SpeakFormat"></param>
        public TimelineAsset CreateMessageTimeLine(string timelineName, string Dialogue, string SpineAssetName, string SpeakAssetName,float SectionLength,int ChangeMouthThreshold, string SpeakFormat = ".wav")
        {
            if (File.Exists($@"{saveDirectory}\{timelineName}.playable"))
            {
                Debug.Log($@"{saveDirectory}\{timelineName}.playable拥有文件，跳过");
                return null;
            }
            TimelineAsset timelineAsset = ScriptableObject.CreateInstance<TimelineAsset>();//创建一个资源
            AssetDatabase.CreateAsset(timelineAsset, $@"{saveDirectory}\{timelineName}.playable");
            //先创建资源到本地，AssetDatabase源码里有检测资源函数，不这样他不会写到本地。
            #region 创建资源
            SpineAnimationStateTrack spineBodyTrack = timelineAsset.CreateTrack<SpineAnimationStateTrack>("Spine Body");//身体
            SpineAnimationStateTrack spineSpecialTrack = timelineAsset.CreateTrack<SpineAnimationStateTrack>("Spine Special");//特殊操作
            SpineAnimationStateTrack spineFaceTrack = timelineAsset.CreateTrack<SpineAnimationStateTrack>("Spine Face");//脸
            SpineAnimationStateTrack spineMouthTrack = timelineAsset.CreateTrack<SpineAnimationStateTrack>("Spine Lip");//嘴 
            spineBodyTrack.trackIndex = 0;
            spineSpecialTrack.trackIndex = 1;
            spineFaceTrack.trackIndex = 2;
            spineMouthTrack.trackIndex = 3;
            DialogueControlTrack dialogueControlTrack = timelineAsset.CreateTrack<DialogueControlTrack>("Dialog");
            AudioTrack SpeakTrack = timelineAsset.CreateTrack<AudioTrack>("Speak");
            AudioTrack SoundTrack = timelineAsset.CreateTrack<AudioTrack>("Sound");
            #endregion
            #region 音频与嘴型处理
            string audioPath = $@"{DialogueDirectiory}\{SpeakAssetName}{SpeakFormat}";
            float clipLength = 0;

            AudioClip Speak = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath);
            if (Speak != null)
            {
                SpeakTrack.CreateClip(Speak);
                string MouthReferenceAssetsPath = $@"{SpineDirectiory}\{SpineAssetName}\ReferenceAssets";
                LipTrackExtension.MouthGroup[] group;
                if (CacheMouthGroups.ContainsKey(MouthReferenceAssetsPath))
                {
                    group = CacheMouthGroups[MouthReferenceAssetsPath];
                }
                else
                {
                    // var MousthAsset = AssetDatabase.LoadAllAssetsAtPath(MouthReferenceAssetsPath);//不能用全域载入，会把所有动画加载进来，但只需要口型资源
                    string[] guids = AssetDatabase.FindAssets("Mouth", new string[] { MouthReferenceAssetsPath });//嘴型所在位置
                    group = LipTrackExtension.MouthGroup.ToGroupArrary(guids);//对口型分组
                    if(group.Length==0)
                    {
                        Debug.LogError(SpineAssetName + "未生成任何可用口型，请给spine中制作Mouth动画，动画名称为MouthC{序号}，MouthO{序号}，序号为口型强弱");
                        return null;
                    }
                    CacheMouthGroups.Add(MouthReferenceAssetsPath, group);
                }
                LipTrackExtension.ProcessLipAmplitudePro(ref spineMouthTrack, group, Speak, SectionLength, ChangeMouthThreshold);
                clipLength = Speak.length;
            }
            else
            {
                Debug.Log(timelineName + "不包含音频资源");
            }
            #endregion
            #region 文本处理
            TimelineClip clip = dialogueControlTrack.CreateClip<DialogueControlClip>();
            DialogueControlClip clipResource = (DialogueControlClip)clip.asset;
            clipResource.template.dialogue = Dialogue;
            clip.displayName = timelineName;
            if (clipLength != 0)
            {
                clip.duration = clipLength;
            }
            else
            {
                clipLength = Dialogue.Length * 0.12f;
                clip.duration = clipLength;
            }
            #endregion
            #region 动画处理
            AnimationReferenceAsset body = AssetDatabase.LoadAssetAtPath<AnimationReferenceAsset>($@"{SpineDirectiory}\{SpineAssetName}\ReferenceAssets\Idle.asset");
            if (body != null)
            {
                SpineAnimationStateClip spineClip = InputStateTrack(ref spineBodyTrack, body, clipLength);
                spineClip.template.loop = true;
            }
            else
            {
                Debug.Log("找不到默认的Idle动画");
            }
            #endregion
            return timelineAsset;
        }
        private SpineAnimationStateClip InputStateTrack(ref SpineAnimationStateTrack spineAnimationStateTrack, AnimationReferenceAsset reference, float duration)
        {
            TimelineClip clip = spineAnimationStateTrack.CreateClip<SpineAnimationStateClip>();//创建一个节拍
            SpineAnimationStateClip clipResource = (SpineAnimationStateClip)clip.asset;//设置节拍为Clip
            clipResource.template.animationReference = reference;//设置资源
            clip.displayName = reference.name;
            clip.duration = duration;
            return clipResource;
        }


        private SpineAnimationStateClip InputStateTrack(ref SpineAnimationStateTrack spineAnimationStateTrack, AnimationReferenceAsset reference)
        {
            TimelineClip clip = spineAnimationStateTrack.CreateClip<SpineAnimationStateClip>();//创建一个节拍
            SpineAnimationStateClip clipResource = (SpineAnimationStateClip)clip.asset;//设置节拍为Clip
            clipResource.template.animationReference = reference;//设置资源
            clip.displayName = reference.name;
            clip.duration = reference.Animation.Duration > 0.2f ? reference.Animation.Duration : 0.2f;
            return clipResource;
        }

        private TimelineClip[] InputStateTracks(ref SpineAnimationStateTrack spineAnimationStateTrack, params AnimationReferenceAsset[] references)
        {
            double timing = 0;
            TimelineClip[] timelineClips = new TimelineClip[references.Length];
            for (int i = 0, Length = references.Length; i < Length; i++)
            {
                TimelineClip clip = spineAnimationStateTrack.CreateClip<SpineAnimationStateClip>();//创建一个节拍
                SpineAnimationStateClip data = (SpineAnimationStateClip)clip.asset;//设置节拍为Clip
                data.template.animationReference = references[i];//设置资源
                clip.displayName = references[i].name;
                clip.duration = references[i].Animation.Duration > 0.2f ? references[i].Animation.Duration : 0.2f;
                clip.start = timing;
                timelineClips[i] = clip;
                timing += data.duration;
            }
            return timelineClips;
        }

    }
    /// <summary>
    /// 注意函数名与QQTimelineCreaterExtend中函数相同
    /// </summary>
    public static class QQTimelineShowExtend
    {
        //public static void CameraFocusRole(object Binding)
        //{
        //    //CameraFocusTrack focus = timelineAsset.CreateTrack<CameraFocusTrack>("focus");
        //    //CameraFocusClip clip = (CameraFocusClip)(focus.CreateClip<CameraFocusClip>().asset);
        //}
    }

}
//#endif