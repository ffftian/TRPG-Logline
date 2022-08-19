using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Miao;
using Sirenix.Utilities.Editor;
using UnityEngine.Timeline;
using System.Collections;
using System.Reflection;
using System;

public class MethodData
{
    public bool enable;
    [ReadOnlyAttribute]
    [DisplayAsString(true)]
    //public string methodInfo;
    public MethodInfo methodInfo;
    //[ReadOnlyAttribute]
    [DisplayAsString]//这样可以改为只显示数据
    public string explain;
}

/// <summary>
/// QQLogTimeline创建器，需要重构成设置地址读取的版本。
/// </summary>
public class QQLogTimelineOdinEditorWindow : OdinEditorWindow
{
    [Title("要进行生成的数据")]
    public QQMessageAsset QQMessageDATA;
    protected QQLogSettings settings;
    protected QQTimelineCreater timelineCreater;
    /// <summary>
    /// 初始消息长度。
    /// </summary>
    private int messageCount
    {
        get
        {
            if (QQMessageDATA == null)
            {
                return 0;
            }
            return QQMessageDATA.messageDataList.Count;
        }
    }
    [Tooltip("比如角色骨骼动画都使用的同一种，依靠皮肤创建预制体区分角色，则打勾自行输入Spine骨骼名称")]
    public bool 使用默认角色名索引的SpineAsset;
    //[Tooltip("如果所有角色骨骼动画使用同一种，则使用该名字来索引查找SpineAsset")]
    [HideIf("使用默认角色名索引的SpineAsset")]
    public string 索引的SpineAsset名称;
    [Tooltip("不想生成动画轨道的名称，像kp/dm/围观者等没主要实体者使用")]
    public string[] 需忽略的QQ昵称;

    [TableList(ShowIndexLabels = true,IsReadOnly = true)]
    public List<MethodData> 额外TimeLine脚本;
    [Tooltip("负责生成轨道中，已有参数的高级设置")]
    public TimelineCreaterSettings 生成轨道高级设置;
    //[Tooltip("可启用的用于TimeLine生成时额外进行函数调用")]
    //[ValueDropdown("ExtendDropDown")]
    //public List<string> TimeLineExtension;
    //IEnumerable ExtendDropDown;
    //protected MethodInfo[] extendMethodInfos;

    [PropertyOrder(1)]
    public List<MessageData> messageList;
    [InfoBox("载入后，调整这根轴来选取需要处理的message数据范围")]
    [MenuItem("QQ文本编辑器/QQLogTimelineOdin生成器")]
    private static void OpenWindow()
    {
      
        QQLogTimelineOdinEditorWindow instance = GetWindow<QQLogTimelineOdinEditorWindow>("QQLogTimeline生成器");
        
        instance.额外TimeLine脚本 = GetCreaterMethods(ref instance.settings.extendMethodInfos);
    }

    private static List<MethodData> GetCreaterMethods(ref List<bool> extendMethodInfos)
    {
        List<MethodData> creater = new List<MethodData>();
        List<int> indexs = new List<int>();


       //var test=   typeof(QQTimelineCreaterExtension).GetMethods(BindingFlags.Static | BindingFlags.Public);
       // var t2 = test[0].GetCustomAttributes(true);
        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
        {
            foreach (var method in t.GetMethods(BindingFlags.Static| BindingFlags.Public))
            {
                TimeLineCreaterAttribute timeLineCreaterAttribute = method.GetCustomAttribute<TimeLineCreaterAttribute>(true);
                if (timeLineCreaterAttribute != null)
                {
                    bool insert = false;
                    for (int i = 0; i < indexs.Count; i++)
                    {
                        if (indexs[i] > timeLineCreaterAttribute.index)
                        {
                            //好像是插在某个序号之前。
                            indexs.Insert(i-1, timeLineCreaterAttribute.index);
                            creater.Insert(i - 1, new MethodData() { methodInfo = method,explain = timeLineCreaterAttribute.explain }) ;
                            insert = true;
                            break;
                        }
                    }
                    if(!insert)
                    {
                        indexs.Add(indexs.Count);
                        creater.Add(new MethodData() { methodInfo = method, explain = timeLineCreaterAttribute.explain });
                    }
                }
            }
        }
        ///同步启用情况
        for(int i=0;i< creater.Count;i++)
        {
            if (i < extendMethodInfos.Count)
            {
                creater[i].enable = extendMethodInfos[i];
            }
        }
        return creater;

    }

    /////////////////Editor/////////////////////
    [PropertyOrder(-2)]
    [Button("刷新消息资源显示")]
    public void RefreshMessageAssetPath()
    {
        if (QQMessageDATA == null)
        {
            messageList.Clear();
            messageListRange = Vector2Int.zero;

        }
        else
        {
            //linq函数处理过的数组是浅拷贝，数组的更改将同步到messageAsset中。
            messageList = QQMessageDATA.messageDataList.Skip(messageListRange.x).Take(messageListRange.y - messageListRange.x).ToList();
        }
    }
    [PropertyOrder(-1)]
    [Button("根据范围创建TimeLine")]
    public void CreateTimeLine()
    {
        AssetDatabase.CreateFolder(QQLogSettings.timeLineDirectory, QQMessageDATA.name);
        string TimeLinePath = $@"Assets\Resources\{QQLogSettings.timeLineDirectory}\{QQMessageDATA.name}";
        timelineCreater = new QQTimelineCreater(TimeLinePath);
        for (int i = 0; i < messageList.Count; i++)
        {
            string spineAssetName = 使用默认角色名索引的SpineAsset ? messageList[i].roleName : 索引的SpineAsset名称;
            if (需忽略的QQ昵称.Contains(messageList[i].roleName))
            {
                spineAssetName = null;
            }
            TimelineAsset timelineAsset = timelineCreater.CreateMessageTimeLine(messageList[i].fileID, messageList[i].log, spineAssetName, $@"{messageList[i].roleName}\{messageList[i].fileID}",生成轨道高级设置.SectionLength,生成轨道高级设置.ChangeMouthThreshold);

            for(int j =0; j< 额外TimeLine脚本.Count; j++)
            {
                if(额外TimeLine脚本[j].enable)
                {
                    额外TimeLine脚本[j].methodInfo.Invoke(null, new object[] { spineAssetName,timelineAsset, messageList[i] });
                }
            }
        }
        //高亮提示创建完成。
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(TimeLinePath));
    }

    [MinMaxSlider(0, "messageCount", true)]
    public Vector2Int messageListRange;


    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void DrawEditor(int index)
    {
        base.DrawEditor(index);
    }

    protected override void Initialize()
    {
        base.Initialize();
        settings = QQLogSettings.LoadSettings();
        生成轨道高级设置 = TimelineCreaterSettings.LoadSettings();
        LipTrackExtension.MaxThreshold = 生成轨道高级设置.MaxThreshold;
        LipTrackExtension.MinThreshold = 生成轨道高级设置.MinThreshold;
        //if (settings == null)
        //{
        //    settings = ScriptableObject.CreateInstance<QQLogSettings>();
        //    AssetDatabase.CreateFolder("Assets", QQLogSettings.solderPath);
        //    AssetDatabase.CreateAsset(settings, QQLogSettings.settingPath + QQLogSettings.settingName);
        //    AssetDatabase.CreateFolder("Assets", QQLogSettings.timeLineDirectory);
        //}
        if (settings.QQMessageAsset != null)
        {
            QQMessageDATA = settings.QQMessageAsset;
            //messageAsset = AssetDatabase.LoadAssetAtPath<QQMessageAsset>(settings.qqMessageAssetPath);
            messageListRange = settings.messageListRange;
            RefreshMessageAssetPath();
        }
        使用默认角色名索引的SpineAsset = settings.UseDefaultName;
        索引的SpineAsset名称 = settings.SpineAssetName;
        需忽略的QQ昵称 = settings.IgnoreName;
        //TimeLineExtension = settings.TimeLineExtension;
        //CameraFocusRole = settings.CameraFocusRole;
    }




    protected void OnDisable()
    {
        //if (QQMessageDATA != null)
        //{
        settings.messageListRange = messageListRange;
        settings.QQMessageAsset = QQMessageDATA;
        //}
        settings.UseDefaultName = 使用默认角色名索引的SpineAsset;
        settings.SpineAssetName = 索引的SpineAsset名称;
        settings.IgnoreName = 需忽略的QQ昵称;

        settings.extendMethodInfos = 额外TimeLine脚本.Select(v => v.enable).ToList();

        //settings.TimeLineExtension = TimeLineExtension;
        //settings.CameraFocusRole = CameraFocusRole;

        EditorUtility.SetDirty(settings);
    }
}
