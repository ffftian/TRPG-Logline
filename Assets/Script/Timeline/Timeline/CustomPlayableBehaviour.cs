using UnityEngine.Playables;


public class CustomPlayableBehaviour : PlayableBehaviour
{
    internal double Time => playable.GetTime();
    internal double Duration => playable.GetDuration();
    internal double Progress => Time / Duration;
    private bool isInit = false;
    private bool isStay = false;
    private Playable playable;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        base.ProcessFrame(playable, info, playerData);

        this.playable = playable;

        if (!isInit) Init(playerData);
        if (!isStay) { isStay = true; OnEnter(); }
        OnStay();
    }
    //销毁时调用
    public override void OnPlayableDestroy(Playable playable)
    {
        base.OnPlayableDestroy(playable);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        base.OnBehaviourPause(playable, info);

        this.playable = playable;
        // 暂停时，如果时间条在playable之外，则重置
        if ((0f >= Progress || Progress >= 1f) && isStay) { OnExit(); isStay = false; }
    }

    /// <summary>
    /// 首次进入时调用
    /// </summary>
    /// <param name="playerData"></param>
    protected virtual void Init(object playerData) { }
    /// <summary>
    /// 进入时调用
    /// </summary>
    protected virtual void OnEnter() { }
    /// <summary>
    /// 在timeline中时，每帧调用
    /// </summary>
    protected virtual void OnStay() { }
    /// <summary>
    /// 退出时调用
    /// </summary>
    protected virtual void OnExit() { }

}