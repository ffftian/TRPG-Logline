using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.RegularExpressions;


public static class QQTool
{



    public static List<TText> QQLogSplit<TText>(string text) where TText : TextData, new()
    {
        

        string[] SingleConversation = Regex.Split(text, "(?=\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2})");//这个靠零宽正向先行断言匹配了每一句话前是否是日期格式，问题在于必须跳过第一格。
        List<TText> dataList = new List<TText>();

        for (int i = 1; i < SingleConversation.Length; i++)//略过第一行，第一行固定为空。
        {

            TText txt = new TText();
            txt.Analysis(SingleConversation[i],i);
            dataList.Add(txt);
        }
        return dataList;
    }




    //string a = "2019 - 07 - 31 20:09:43 ftianf(790947404)伊斯科 / 约翰 你们迷糊的醒来，本能的坐了起来，发现自己身处于不明金属制成的舱内，舱内还微微冒着白色烟，你们感到寒冷，注意到全身裸体，因寒冷不想在这舱内多呆一步遍本能的爬了出来";

    //MatchCollection MatchSpilit = Regex.Matches(Text, "[\u4e00-\u9fa5_a-zA-Z0-9]*\n");
    // MatchCollection MatchSpilit = Regex.Matches(Text, "[\\w-:()/ ]*");
    // MatchCollection MatchSpilit = Regex.Matches(Text, "((?!hello).)+$/");
    // MatchCollection MatchSpilit = Regex.Matches(text, "[\\w-:()/，。 ]+");

    // MatchCollection MatchSpilit = Regex.Matches(text, "[\\w-:()/，。 ]+(?=\\n\\r\\n)");
    //MatchCollection MatchSpilit = Regex.Matches(text, "(?<!\\r\\n\\r\\n)[\\w-:()/，。 ]+");
    //MatchCollection MatchSpilit = Regex.Matches(text, "(?<=\\r\\n\\r\\n)[\\w-:()/，。 ]+");


    //    foreach (Match match in MatchSpilit)
    //    {
    //        Debug.Log(match.Value);
    //    }


    //    foreach (string g in G)
    //    {
    //        Debug.Log(g);
    //    }
}

