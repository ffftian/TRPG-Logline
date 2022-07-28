using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using Miao;

/// <summary>
/// 包含头像的文本类
/// </summary>
[Serializable]
public class MessageData : TextData
{
    /// <summary>
    /// 文件所在ID，QQ默认中的:为保存不支持格式
    /// </summary>
    public string fileID;
    public override void Analysis(string SingleText, int Serial)
    {
        base.Analysis(SingleText, Serial);
        fileID = Regex.Replace(this.ID, MiaoRegexTool.路径非法字符, "");
    }


    private Texture2D _headTexture;
    public Texture2D HeadTexture
    {
        get
        {
            return _headTexture == null ?
            new Texture2D(128, 128):  _headTexture;
        }
        set { _headTexture = value; }
    }


}

