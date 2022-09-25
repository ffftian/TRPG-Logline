using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// QQ文本类。
/// </summary>
public class TextData
{
    public string ID;
    public string QQ;
    public string roleName;
    [TextArea]
    public string log;
    public string dateTime;
    /// <summary>
    /// 正则匹配，解析QQ记录。
    /// </summary>
    /// <param name="singleText"></param>
    /// <param name="serial"></param>
    virtual public void Analysis(string singleText,int serial)
    {
        try
        {
            dateTime = Regex.Match(singleText, "\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2} ").Value;//提取时间
            Match QQAndName = Regex.Match(singleText, "(?<=(\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2} )).+\\([0-9]+\\)");//用0宽断言提取名称
            if (QQAndName.Length == 0)
                QQAndName = Regex.Match(singleText, "\\([0-9]+\\)");//当没有用户名时，只提取QQ号

            roleName = QQAndName.Value.Split('(')[0];//提取用户名 
            QQ = QQAndName.Value?.Split('(')[1].Split(')')[0];//提取QQ号

            string[] SingleConversation = Regex.Split(singleText, "\\r\\n", RegexOptions.IgnoreCase);//QQ消息与ID分开
            for (int i = 0; i < SingleConversation.Length; i++)
            {
                switch(i)
                {
                    case 0:
                        ID = SingleConversation[i];//第一行为QQ消息ID
                        break;
                    default:
                        log += SingleConversation[i] + "\n";//剩下的为log
                        break;
                }
            }
        }
        catch
        {
            throw new Exception($"错误的读取，序号{serial}，输出原句:{singleText}");
        }
    }
}

