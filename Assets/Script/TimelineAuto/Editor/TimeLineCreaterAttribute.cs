using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[AttributeUsage(AttributeTargets.Method)]
public class TimeLineCreaterAttribute : Attribute
{
    public int index;
    public string explain;
    public TimeLineCreaterAttribute(int index, string explain)
    {
        this.index = index;
        this.explain = explain;
    }
}

