#if UNITY_EDITOR
using Spine.Unity;
using Spine.Unity.Playables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System.Linq;
using UnityEditor;

public class LipTrackExtension
{

    public class MouthGroup
    {
        public int threshold;
        public AnimationReferenceAsset MouthO;
        public AnimationReferenceAsset MouthC;

        public static MouthGroup[] ToGroupArrary(string[] guids)//先只能写到这里了,再继续就要debug尝试了。假设写完了。
        {
            int MouthGroupCount = guids.Length/2;//将资源转换成debug
            //int number = System.Convert.ToInt32(LastName[LastName.Length - 1]);
            MouthGroup[] group = new MouthGroup[MouthGroupCount];


            for (int i = 0; i < MouthGroupCount; i++)
            {
                MouthGroup mouthAsset = new MouthGroup();
                mouthAsset.MouthC = AssetDatabase.LoadAssetAtPath<AnimationReferenceAsset>(AssetDatabase.GUIDToAssetPath(guids[i]));
                mouthAsset.MouthO = AssetDatabase.LoadAssetAtPath<AnimationReferenceAsset>(AssetDatabase.GUIDToAssetPath(guids[MouthGroupCount + i]));
               
                mouthAsset.threshold =  mouthAsset.MouthC.name[mouthAsset.MouthC.name.Length-1]-48;//因为阿斯克码转换问题
                group[i] = mouthAsset;
            }
            return group;
        }

    }



    static public float MaxThreshold = 0.05f;//阈值在这里这个的样子，这个朝上用最大口型。
    static public float MinThreshold = 0.0015f;//最小阈值，低于这个视为闭嘴

    enum LipState
    {
        默认,
        张嘴,
        闭嘴,
    }

    /// <summary>
    /// 按照提供的开闭口型，来匹配这段对话的程度。
    /// </summary>
    /// <param name="spineMouthTrack"></param>
    /// <param name="MouthOpen"></param>
    /// <param name="MouthClose"></param>
    /// <param name="audio"></param>
    /// <param name="SectionLength=多少秒的声音监听成一份切片长度0.016=1帧"></param>
    /// <param name="Threshold=监听张嘴阈值的长度"></param>//
    public static void ProcessLip(ref SpineAnimationStateTrack spineMouthTrack, AnimationReferenceAsset MouthOpen, AnimationReferenceAsset MouthClose, AudioClip audio, float SectionLength = 0.02f, float Threshold = 0.005f)
    {
        float[] rectifier = SamplesCilp(audio, SectionLength);
        LipState lipState = LipState.默认;

        float Start = 0;//在TimeLine中的长度
        float Duration = 0;//动画持续时间

        for (int i = 0; i < rectifier.Length; i++)
        {
            if (lipState == LipState.默认)
            {
                Start = 0;
                Duration = 0;
                if (rectifier[i] < Threshold)
                {
                    Start = i * SectionLength;
                    Duration += SectionLength;
                    lipState = LipState.闭嘴;
                }
                else
                {
                    Start = i * SectionLength;
                    Duration += SectionLength;
                    lipState = LipState.张嘴;
                }
            }
            else if (lipState == LipState.闭嘴)
            {
                if (rectifier[i] < Threshold)
                {
                    Duration += SectionLength;
                }
                else
                {
                    Duration += SectionLength;
                    lipState = LipState.默认;
                    SetMouth(ref spineMouthTrack, MouthClose, Start, Duration);//结算闭嘴

                }

            }
            else if (lipState == LipState.张嘴)
            {
                if (rectifier[i] < Threshold)
                {
                    Duration += SectionLength;
                    lipState = LipState.默认;
                    SetMouth(ref spineMouthTrack, MouthOpen, Start, Duration);//张嘴结算
                }
                else
                {
                    Duration += SectionLength;
                }
            }
        }
        if (lipState == LipState.闭嘴)
        {
            SetMouth(ref spineMouthTrack, MouthClose, Start, Duration);
        }
        else if (lipState == LipState.张嘴)
        {
            SetMouth(ref spineMouthTrack, MouthOpen, Start, Duration);
        }
    }

    /// <summary>
    /// 按照提供的开闭口型，来匹配这段对话的程度。
    /// </summary>
    /// <param name="spineMouthTrack"></param>
    /// <param name="MouthOpen"></param>
    /// <param name="MouthClose"></param>
    /// <param name="audio=声音本体文件"></param>
    /// <param name="SectionLength=多少秒的声音监听成一份切片长度"></param>
    /// <param name="Threshold=监听张嘴阈值的长度"></param>
    /// <param name="ChangeMouthThreshold=在多少时间下监听到张嘴的情况下视为连贯的张嘴，反之视为闭嘴，单位以SectionLength来定。"></param>
    public static void ProcessLipPro(ref SpineAnimationStateTrack spineMouthTrack, AnimationReferenceAsset MouthOpen, AnimationReferenceAsset MouthClose, AudioClip audio, float SectionLength = 0.02f, float Threshold = 0.005f, int ChangeMouthThreshold = 20)
    {
        float[] rectifier = SamplesCilp(audio, SectionLength);

        LipState lipState = LipState.默认;
        float Start = 0;//在TimeLine中的长度
        float Duration = 0;//动画持续时间
        float ThresholdTime = 0;//计算阈值的时间

        for (int i = 0; i < rectifier.Length; i++)
        {
            if (lipState == LipState.默认)
            {
                Start = 0;
                Duration = 0;
                if (rectifier[i] < Threshold)
                {
                    Start = i * SectionLength;
                    Duration += SectionLength;
                    lipState = LipState.闭嘴;
                }
                else
                {
                    Start = i * SectionLength;
                    Duration += SectionLength;
                    lipState = LipState.张嘴;
                }
            }
            else if (lipState == LipState.闭嘴)//如果阈值高，调用闭嘴结算。
            {
                if (rectifier[i] < Threshold)
                {
                    Duration += SectionLength;
                }
                else
                {
                    Duration += SectionLength;
                    lipState = LipState.默认;
                    //调用闭嘴结算
                    SetMouth(ref spineMouthTrack, MouthClose, Start, Duration);
                    //开始张嘴轮次。
                }

            }
            else if (lipState == LipState.张嘴)
            {
                if (rectifier[i] < Threshold)//只有连续一段时间，声音小于阈值，调用张嘴结束动画。开始闭嘴。
                {
                    //需求是，明明没有声音但装作有声音。
                    //张嘴继续下去。
                    //反之，倒退等同于Threshold的次数，并在次数前设张嘴。
                    //起始时间则也跟着倒退。
                    ThresholdTime++;
                    Duration += SectionLength;
                    if (ThresholdTime == ChangeMouthThreshold)//只有连续一段时间声音小于阈值，才闭嘴。
                    {//需要重新写阈值倒退逻辑一类的东西。
                        Duration = Duration - (SectionLength * ChangeMouthThreshold);
                        SetMouth(ref spineMouthTrack, MouthOpen, Start, Duration);

                        Start = (i * SectionLength) - (SectionLength * (ChangeMouthThreshold - 1));//延后阈值时间
                        Duration = SectionLength * (ChangeMouthThreshold);

                        ThresholdTime = 0;
                        lipState = LipState.闭嘴;


                    }
                }
                else
                {
                    ThresholdTime = 0;
                    Duration += SectionLength;
                }
            }
        }
        if (lipState == LipState.闭嘴)
        {
            SetMouth(ref spineMouthTrack, MouthClose, Start, Duration);
        }
        else if (lipState == LipState.张嘴)
        {
            SetMouth(ref spineMouthTrack, MouthOpen, Start, Duration);
        }
    }
    /*
    /// <summary>
    /// 按照提供的复数口型，来匹配这段对话的程度。
    /// </summary>
    /// <param name="spineMouthTrack"></param>
    /// <param name="LipGroups"></param>
    /// <param name="audio"></param>
    /// <param name="SectionLength=多少秒监听成切片长度"></param>
    /// <param name="ChangeMouthThreshold=在多少时间下监听到张嘴的情况下视为连贯的沿用当前嘴型，反之视为不沿用，单位以SectionLength来定。"></param>
    public static void ProcessLipAmplitude(ref SpineAnimationStateTrack spineMouthTrack, List<AutomaticUnit.MouthGroup> LipGroups, AudioClip audio, float SectionLength = 0.02f, int ChangeMouthThreshold = 4)
    {
        float[] rectifier = SamplesCilp(audio, SectionLength);
        float ThreshMax = 0;//阈值
        AnimationReferenceAsset currentLip = default;
        AnimationReferenceAsset lastLip = default;

        float Start = 0;//在TimeLine中的长度
        float Duration = 0;//动画持续时间
        int ThresholdTime = 0;//沿用表情控制变量

        for (int a = 0; a < LipGroups.Count; a++)
        {
            ThreshMax = LipGroups[a].threshold > ThreshMax ? LipGroups[a].threshold : ThreshMax;

        }
        float 阈值档位 = (MaxThreshold - MinThreshold) / ThreshMax;

        //初始化最初的口型,一般就为默认闭嘴的状态。
        currentLip = LipAnalysis(LipGroups, 阈值档位, rectifier[0]);
        Duration += SectionLength;
        lastLip = currentLip;

        for (int i = 1; i < rectifier.Length; i++)
        {
            Duration += SectionLength;

            if (currentLip != lastLip)//当不同时，截断对照组，开始进行口型设置。
            {
                ThresholdTime++;
                if (ThresholdTime == ChangeMouthThreshold)//只有连续一段时间声音小于阈值，才闭嘴。
                {
                    Duration = Duration - (SectionLength * ChangeMouthThreshold);
                    SetMouth(ref spineMouthTrack, lastLip, Start, Duration);
                    Start = (i * SectionLength) - (SectionLength * (ChangeMouthThreshold - 1));//延后阈值时间
                    Duration = SectionLength * (ChangeMouthThreshold);
                    lastLip = currentLip;
                    //Start = i * SectionLength;
                    //Duration = 0;
                    ThresholdTime = 0;
                }
            }
            currentLip = LipAnalysis(LipGroups, 阈值档位, rectifier[i]);

        }
        SetMouth(ref spineMouthTrack, currentLip, Start, Duration);
        //第一步，按照声音的整体文件建立声音阈值组，人声音的波形应该是没有大区别的，所以采样率应是人听过后系统自带的。
        //第二步，寻找提供中口型，最接近的那个口型，从小到大排列。
    }
    */
    /// <summary>
    /// 按照提供的复数口型，来匹配这段对话的程度，0档位统一为闭嘴嘴型,当{X2}转{X1}时，会自动调用对应闭嘴口型。
    /// 必须保证LipGroup组为从小到大。
    /// </summary>
    /// <param name="spineMouthTrack"></param>
    /// <param name="LipGroup"></param>
    /// <param name="audio"></param>
    /// <param name="SectionLength"></param>
    /// <param name="ChangeMouthThreshold"></param>
    public static void ProcessLipAmplitudePro(ref SpineAnimationStateTrack spineMouthTrack, LipTrackExtension.MouthGroup[] LipGroup, AudioClip audio, float SectionLength = 0.02f, int ChangeMouthThreshold = 4)
    {
        //AutomaticUnit.MouthGroup CloseLipGroup = LipGroup[0];
        //IEnumerable<AutomaticUnit.MouthGroup> OpenLipGroup = from Lip in LipGroup where Lip.threshold > 0 select Lip;
        //移除0号口型组，作为独立关闭需要。

        float[] rectifier = SamplesCilp(audio, SectionLength);
        float ThreshMax = 0;//最大格式阈值
        AnimationReferenceAsset currentLip = default;
        AnimationReferenceAsset lastLip = default;

        float Start = 0;//在TimeLine中的长度
        float Duration = 0;//动画持续时间
        int ThresholdTime = 0;//沿用表情控制变量

        //优化，最大阈值总为LipGroup的最后一位。
        ThreshMax = LipGroup[LipGroup.Length - 1].threshold;
        float 每档实际增长值 = (MaxThreshold - MinThreshold) / ThreshMax;

        //初始化最初的口型,一般就为默认闭嘴的状态。
        currentLip = LipGroup[0].MouthC;
        Duration += SectionLength;
        lastLip = currentLip;

        for (int i = 1; i < rectifier.Length; i++)
        {
            Duration += SectionLength;

            if (currentLip != lastLip)//当不同时，截断对照组，开始进行口型设置。
            {
                ThresholdTime++;
                if (ThresholdTime == ChangeMouthThreshold)//只有连续一段时间声音小于阈值，才闭嘴。
                {
                    Duration = Duration - (SectionLength * ChangeMouthThreshold);
                    SetMouth(ref spineMouthTrack, lastLip, Start, Duration);
                    Start = (i * SectionLength) - (SectionLength * (ChangeMouthThreshold - 1));//延后阈值时间
                    Duration = SectionLength * (ChangeMouthThreshold);
                    lastLip = currentLip;
                    //Start = i * SectionLength;
                    //Duration = 0;
                    ThresholdTime = 0;
                }
            }
            currentLip = LipAnalysisPro(lastLip, LipGroup, 每档实际增长值, rectifier[i]);

        }
        SetMouth(ref spineMouthTrack, currentLip, Start, Duration);
        //第一步，按照声音的整体文件建立声音阈值组，人声音的波形应该是没有大区别的，所以采样率应是人听过后系统自带的。
        //第二步，寻找提供中口型，最接近的那个口型，从小到大排列。
    }

    /// <summary>
    /// 解析出最接近阈值的Lip。
    /// </summary>
    /// <param name="LipGroups"></param>
    /// <param name="thresholdGrowth=阈值增长"></param>
    /// <param name="rectifier=当前音量阈值"></param>
    /// <returns></returns>
    [System.Obsolete("使用LipAnalysisPro,这个方法不自动纠正闭嘴口型，只做研发参考，不推荐使用")]
    private static AnimationReferenceAsset LipAnalysis(LipTrackExtension.MouthGroup[] LipGroups, float thresholdGrowth, float rectifier)
    {
        AnimationReferenceAsset Lip = default;
        float Proximal = 100;//Proximal近端的，最近的 获取增长阈值
        if (rectifier < MinThreshold)
        {
            return LipGroups[0].MouthC;//小于阈值，证明是空的。
        }
        for (int i = 0; i < LipGroups.Length; i++)
        {
            float tempProximal = Mathf.Abs(LipGroups[i].threshold * thresholdGrowth + MinThreshold - rectifier);//因为是abs，越接近0效果越好。
            if (tempProximal < Proximal)
            {
                Proximal = tempProximal;
                Lip = LipGroups[i].MouthO;
            }
        }
        return Lip;
    }

    /// <summary>
    /// 使用最接近阈值的Lip。
    /// </summary>
    /// <param name="LastLip=上一个使用的口型"></param>
    /// <param name="OpenLipGroups"></param>
    /// <param name="CloseLipGroups"></param>
    /// <param name="thresholdGrowth==每档位阈值增长速度,既(MaxThreshold - MinThreshold) / ThreshMax"></param>
    /// <param name="rectifier==被整流过的当前声音所在位值"></param>
    /// <returns></returns>
    private static AnimationReferenceAsset LipAnalysisPro(AnimationReferenceAsset LastLip, LipTrackExtension.MouthGroup[] LipGroups, float thresholdGrowth, float rectifier)
    {
        AnimationReferenceAsset Lip = default;
        if (rectifier < MinThreshold)//小于最小值，证明是处于闭嘴阈值内的。
        {
            //返回闭嘴融合上一帧口型。
            string LastName = LastLip.name;
            int number = System.Convert.ToInt32(LastName[LastName.Length - 1]-48);
            for (int i = 0; i < LipGroups.Length; i++)
            {
                if(number == LipGroups[i].threshold)
                {
                    return LipGroups[i].MouthC;
                }
            }
            Debug.LogError($"未检测到{number}号口型，请检查动画命名");
        }

        float Proximal = 100;//Proximal近端的，最近的 获取增长阈值
        int target=0;
        for (int i=0; i< LipGroups.Length;i++)
        {
            float tempProximal = Mathf.Abs(LipGroups[i].threshold * thresholdGrowth + MinThreshold - rectifier);//因为是abs，越接近0证明越好。
            if (tempProximal < Proximal)
            {
                Proximal = tempProximal;
                target = i;
            }
            else
            {
                //如果小于ABS的趋近值，直接返回。
                break;
            }
        }
        Lip = LipGroups[target].MouthO;
        return Lip;
    }



    private static void SetMouth(ref SpineAnimationStateTrack spineMouthTrack, AnimationReferenceAsset Mouth, float Start, float Duration)
    {
        var Clip = CreateClip(ref spineMouthTrack, Mouth);
        Clip.start = Start;
        Clip.duration = Duration;

    }



    private static TimelineClip CreateClip(ref SpineAnimationStateTrack AnimationStateTrack, AnimationReferenceAsset reference)
    {
        TimelineClip clip = AnimationStateTrack.CreateClip<SpineAnimationStateClip>();//创建一个节拍
        SpineAnimationStateClip data = (SpineAnimationStateClip)clip.asset;//设置节拍为Clip
        data.template.animationReference = reference;//设置资源
        clip.displayName = reference.name;
        return clip;
        // clip.duration = reference.Animation.Duration > 0.2f ? reference.Animation.Duration : 0.2f;
    }
    /// <summary>
    /// 求出当前声音按照时间下的切片=采样率。
    /// </summary>
    /// <param name="audio"></param>
    /// <param name="Time"></param>
    /// <returns></returns>
    private static float[] SamplesCilp(AudioClip audio, float Time = 0.05f)
    {
        float[] samples = new float[(int)(audio.samples * audio.channels)];//总音频数，双声道以两倍计算。
        audio.GetData(samples, 0);
        //frequency,频率，每秒提取声音多少次41000档位为dvd音质。有多声道取样还要翻倍，所以我们这里计算时长值除channels（通道）

        float 段落大小 = (int)(audio.frequency * Time);
        float[] AggregateSamples = new float[(int)(samples.Length / 段落大小 / audio.channels)];//聚合的样品声波大小。


        float AllWaveform = 0;
        float Average = 0;
        //可能是低频和高频吧。所以只拿一个半波会有问题大概。
        for (int i = 0, i2 = 0, cilpi = 0; i < samples.Length; i++, i2++)
        {
            AllWaveform += Mathf.Abs(samples[i]);
            if (i2 == samples.Length / AggregateSamples.Length)
            {

                Average = AllWaveform / (samples.Length / AggregateSamples.Length);
                AggregateSamples[cilpi] = Average;
                i2 = 0;
                AllWaveform = 0;
                Average = 0;
                cilpi++;
            }
        }
        return AggregateSamples;
    }

    /*
    private static float[] SamplesCilp(AudioClip audio, float Time = 0.05f)
    {
        float[] samples = new float[(int)(audio.samples * audio.channels)];//总音频数，双声道以两倍计算。
        audio.GetData(samples, 0);
        //frequency,频率，每秒提取声音多少次41000档位为dvd音质。有多声道取样还要翻倍，所以我们这里计算时长值除channels（通道）

        float 段落大小 = (int)(audio.frequency * Time);
        float[] AggregateSamples = new float[(int)(samples.Length / 段落大小/audio.channels)];//聚合的样品声波大小。

        float PositiveWaveform = 0;//当前正采样
        float NegativeWaveform=0;//当前逆采样
        float Average=0;
        //可能是低频和高频吧。所以只拿一个半波会有问题大概。
        for (int i = 0, i2 = 0, cilpi = 0; i < samples.Length; i++, i2++)
        {
            if (samples[i] > 0)
            {
                PositiveWaveform += samples[i];
            }
            else
            {
                NegativeWaveform += samples[i];
            }

            if (i2 == samples.Length / AggregateSamples.Length)
            {

                Average = PositiveWaveform / (samples.Length / AggregateSamples.Length);
                AggregateSamples[cilpi] = Average;
                i2 = 0;
                PositiveWaveform = 0;
                NegativeWaveform = 0;
                Average = 0;
                cilpi++;
            }
        }
        return AggregateSamples;
    }
    */



}
#endif