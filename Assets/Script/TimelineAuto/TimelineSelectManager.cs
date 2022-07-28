#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Playables;
using UnityEngine.Timeline;
/// <summary>
/// 读取所有需要预读的脚本
/// </summary>
public static class TimelineSelectManager
{
    public static List<MethodInfo> TimelineSelectMethod;


    public static void TimelineSelectMethodCall(DialogComponent dialogComponent,MessageData serialData, PlayableDirector playableDirector, TrackAsset trackAsset)
    {
        for(int i= 0; i < TimelineSelectMethod.Count; i++)
        {
            TimelineSelectMethod[i].Invoke(null, new object[] { dialogComponent, serialData, playableDirector, trackAsset });
        }
    }

    static TimelineSelectManager()
    {
        TimelineSelectMethod = new List<MethodInfo>();
        foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
        {
            foreach (MethodInfo method in t.GetMethods(BindingFlags.Static | BindingFlags.Public))
            {
                TimelineSelectAttribute timeLineSelect = method.GetCustomAttribute<TimelineSelectAttribute>(true);
                if(timeLineSelect!=null)
                {
                    TimelineSelectMethod.Add(method);
                }
            }
         }
    }
}

#endif

