using Spine.Unity;
using Spine.Unity.Playables;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;
/// <summary>
/// DialogComponent扩展
/// 会自动显示 <paramref name="component.messageAsset"/>中的文本信息。
/// 会自动寻找与 <paramref name="component.messageAsset"/>名称一致的Timeline路径并附加于Playable之中。
/// </summary>
[CustomEditor(typeof(DialogComponent))]
public class DialogComponentEditor : Editor
{
    private DialogComponent component;

    public Action OnSelectLeaveMessage;

    private int serialPtr
    {
        get {  return component.serialPtr;  }
        set
        {
            if (value != component.serialPtr)
            {
                component.OnTimeLineLeaveInvoke();
                component.SelectMessage(value);
            }
                component.serialPtr = value;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!Application.isPlaying)
            DrawExtra();
    }

    public void DrawExtra()
    {
        if (component.messageAsset != null)
        {
            serialPtr = (int)EditorGUILayout.Slider("文本条", serialPtr, 0, component.messageAsset.messageDataList.Count - 1);
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(serialPtr == 0 ? true : false);
            if (GUILayout.Button("文本向左", EditorStyles.miniButtonLeft))
                serialPtr--;
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(serialPtr == component.messageAsset.messageDataList.Count - 1 ? true : false);
            if (GUILayout.Button("文本向右", EditorStyles.miniButtonRight))
                serialPtr++;
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
            MessageData checkedMeesage = component.messageAsset.messageDataList[serialPtr];
            EditorGUILayout.TextField(checkedMeesage.ID);
            EditorGUILayout.TextField(checkedMeesage.roleName);
            EditorGUILayout.TextArea(checkedMeesage.log, GUI.skin.textArea);
            if(GUILayout.Button("引用丢失资源"))
            {
                ReferenceMissingResource();

            }

            if (component.useTimeLineAsset == null)
            {
                string timelinePath = $@"Assets\Resources\{QQLogSettings.timeLineDirectory}\{component.messageAsset.name}\{checkedMeesage.fileID}.playable";
                EditorGUILayout.LabelField($@"Timeline未能找到:{timelinePath}");
            }
        }
    }
    void ReferenceMissingResource()
    {

        QQLogSettings settings = QQLogSettings.LoadSettings();

        foreach (TrackAsset trackAsset in component.useTimeLineAsset.GetRootTracks())
        {
            if(trackAsset is SpineAnimationStateTrack)
            {
                SpineAnimationStateTrack track = (SpineAnimationStateTrack)trackAsset;
                foreach (TimelineClip spineAnimationStateClip in track.GetClips())
                {

                    SpineAnimationStateClip clip = (SpineAnimationStateClip)spineAnimationStateClip.asset;//clip自身反而不能强转。但Clip就是clip
                    if (settings.UseDefaultName)
                    {

                        clip.template.animationReference = LoadAnimation(component.gameObject.name, spineAnimationStateClip.displayName);
                    }
                    else
                    {
                        clip.template.animationReference = LoadAnimation(settings.SpineAssetName, spineAnimationStateClip.displayName);
                    }
                }

            }
        }
        component.SelectMessage(serialPtr);//重新载入资源
    }


    protected AnimationReferenceAsset LoadAnimation(string SpineAssetName,string SpineName)
    {
        string SpineDirectiory = @"Assets\SpineAsset";
        return AssetDatabase.LoadAssetAtPath<AnimationReferenceAsset>($@"{SpineDirectiory}\{SpineAssetName}\ReferenceAssets\{SpineName}.asset");
    }


    private void OnEnable()
    {
        component = (DialogComponent)target;
    }

}

