using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TyperDialogue : BaseMeshEffect
{
    //用于记录当前到了哪个字
    private int _index = 0;
    //记录开始时间
    private float _startTime = 0;
    //是否开始显示打字
    public bool IsPlaying { get; private set; } = false;
    //要显示的字符串
    private string _textString = "";
    //每个字的出现间隔
    private float _interval = 0f;
    // 跳过对话模式的每个字间隔
    private float _skipInterval = 0.02f;
    // 默认间隔
    private float _originInterval = 0f;
    //要显示的Text组件
    private Text _text;
    private Text Text { get { if (!_text) { _text = GetComponent<Text>(); } return _text; } }

    void Update()
    {
        if (IsPlaying)
        {
            while (Time.time - _startTime > _interval)
            {
                _startTime += _interval;
                if (_index < _textString.Length)
                {
                    _index++;
                }
                else
                {
                    _index = _textString.Length;
                    IsPlaying = false;
                }
            }
            UpdateText();
        }
    }

    public void UpdateText() { graphic.SetVerticesDirty(); }

    /// <summary>
    /// 重写ModifyMesh方法，更新文字顶点
    /// </summary>
    /// <param name="vh"></param>
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;

        List<UIVertex> vertexList = new List<UIVertex>();
        vh.GetUIVertexStream(vertexList);
        vertexList = SetTextVertex(vertexList);
        vh.Clear();
        vh.AddUIVertexTriangleStream(vertexList);
    }

    /*
      设置要显示的文字的顶点
    */
    private List<UIVertex> SetTextVertex(List<UIVertex> vertexList)
    {
        List<UIVertex> tmpVertexList = new List<UIVertex>();
        int count = _index * 6;
        count = Mathf.Min(vertexList.Count, count);
        for (int i = 0; i < count; i++)
        {
            tmpVertexList.Add(vertexList[i]);
        }
        return tmpVertexList;
    }

    public void Init(float originSpeed = 5f, float skipSpeed = 20f)
    {
        _originInterval = _interval = 1f / originSpeed;
        _skipInterval = 1f / skipSpeed;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="text">要显示的完整文本</param>
    /// <param name="speed">每秒显示多少个字</param>
    public void SetText(string text)
    {
        _startTime = Time.time;
        _index = 0;
        Text.text = _textString = text;
    }

    public void StartTyping()
    {
        IsPlaying = true;
    }

    public void StopTyping()
    {
        IsPlaying = false;
    }

    public void Skip() { IsPlaying = false; }

    public void SpeedUp(bool isOn)
    {
        if (isOn)
        {
            _interval = _skipInterval;
        }
        else
        {
            _interval = _originInterval;
        }
    }

    public void Finish() { SetIndex(_textString.Length); }

    public void SetIndex(int index)
    {
        _index = index;
    }

}