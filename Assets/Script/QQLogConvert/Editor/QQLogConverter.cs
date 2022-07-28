using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 将txt文本转换为<paramref name="TextAsset"/>序列化化保存和读取的格式。
/// </summary>
public class QQLogConverter : EditorWindow
{
    private Rect rect;
    private TextAsset qqTextAsset;
    private string txtPath = string.Empty;
    private string outputFolderPath = "Assets/AssetDialogue";
    private string waringMessage;
    private bool canBuild = false;

    [MenuItem("QQ文本编辑器/TXT转QQMessageAsset")]
    private static void OpenWindow()
    {
        QQLogConverter instance = GetWindow<QQLogConverter>("TXT转QQMessageAsset");
        instance.minSize = instance.maxSize = new Vector2(400, 400);
    }

    private void OnGUI()
    {
        #region 设置文件
        EditorGUILayout.LabelField("txt文件位置");
        Rect txtRect = EditorGUILayout.BeginHorizontal();
        txtPath = EditorGUILayout.TextField(txtPath);
        qqTextAsset = (TextAsset)EditorGUILayout.ObjectField(qqTextAsset, typeof(TextAsset), false);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 导出文件目录
        Rect folderRect = EditorGUILayout.BeginHorizontal();
        outputFolderPath = EditorGUILayout.TextField("导出文件目录位置", outputFolderPath);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region 导出设置
        EditorGUI.BeginDisabledGroup(!canBuild);
        if (GUILayout.Button("开始转换txt为QQMessageAsset", EditorStyles.miniButtonMid))
        {
            TxtToQQLogData();
        }
        EditorGUI.EndDisabledGroup();
        EditorGUI.BeginDisabledGroup(txtPath.Length == 0 || outputFolderPath.Length == 0 || !File.Exists($@"{outputFolderPath}\{qqTextAsset.name}.asset"));
        {
            if (GUILayout.Button("强制覆盖txt为QQMessageAsset", EditorStyles.miniButtonMid))
            {
                TxtOverrideQQLogData();
            }
        }
        EditorGUI.EndDisabledGroup();
        #endregion

        #region 拖拽碰撞处理

        Object[] dargPath = DragAndDrop.objectReferences;
        int objectLength = dargPath.Length;
        if (objectLength != 0)
        {
            Event currentEvent = Event.current;
            if (txtRect.Contains(currentEvent.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (currentEvent.type == EventType.DragExited)
                {
                    if (dargPath[0] is TextAsset)
                    {
                        qqTextAsset = (TextAsset)dargPath[0];
                        txtPath = DragAndDrop.paths[0];
                    }
                }
            }
            if (folderRect.Contains(currentEvent.mousePosition))
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (currentEvent.type == EventType.DragExited)
                {
                    if (dargPath[0] is DefaultAsset)
                    {
                        outputFolderPath = DragAndDrop.paths[0];
                    }
                }
            }
        }
        #endregion

        #region 导出提示设置
        if (txtPath.Length == 0 || outputFolderPath.Length == 0)
        {
            canBuild = false;
            waringMessage = "参数不完全";
        }
        else if (File.Exists($@"{outputFolderPath}\{qqTextAsset.name}.asset"))
        {
            canBuild = false;
            waringMessage = "文件已包含";
        }
        else
        {
            canBuild = true;
            waringMessage = "可以构建";
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.HelpBox(waringMessage, canBuild ? MessageType.Info : MessageType.Error);
        EditorGUILayout.Space();
        #endregion
    }

    private void TxtToQQLogData()
    {
        QQMessageAsset messageAsset = ScriptableObject.CreateInstance<QQMessageAsset>();
        messageAsset.messageDataList = QQTool.QQLogSplit<MessageData>(qqTextAsset.text);
        AssetDatabase.CreateAsset(messageAsset, $@"{outputFolderPath}\{qqTextAsset.name}.asset");
        EditorGUIUtility.PingObject(messageAsset);
    }
    private void TxtOverrideQQLogData()
    {
        QQMessageAsset messageAsset =  AssetDatabase.LoadAssetAtPath<QQMessageAsset>($@"{outputFolderPath}\{qqTextAsset.name}.asset");
        messageAsset.messageDataList = QQTool.QQLogSplit<MessageData>(qqTextAsset.text);
        EditorUtility.SetDirty(messageAsset);
        EditorGUIUtility.PingObject(messageAsset);
    }


    private void OnEnable()
    {
        outputFolderPath = EditorPrefs.GetString(nameof(QQLogConverter) + nameof(outputFolderPath));
    }
    private void OnDisable()
    {

        EditorPrefs.SetString(nameof(QQLogConverter) + nameof(outputFolderPath), outputFolderPath);
    }

}