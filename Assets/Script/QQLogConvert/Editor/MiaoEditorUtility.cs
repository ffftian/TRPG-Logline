using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class MiaoEditorUtility
{
    /// <summary>
    /// 获取单个的拖拽对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fieldAreaRect"></param>
    /// <param name="asset"></param>
    /// <param name="assetPath"></param>
    public static void GetDargSingle<T>(Rect fieldAreaRect, ref T asset, ref string assetPath) where T : Object
    {
        Object[] dargPath = DragAndDrop.objectReferences;
        int objectLength = dargPath.Length;
        if (objectLength != 0)
        {
            Event currentEvent = Event.current;
            if (fieldAreaRect.Contains(currentEvent.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (currentEvent.type == EventType.DragExited)
                {
                    if (dargPath[0] is T)
                    {
                        //qqTextAsset = (TextAsset)dargPath[0];
                        asset = (T)dargPath[0];
                        assetPath = DragAndDrop.paths[0];
                    }
                }
            }
        }
    }
}

