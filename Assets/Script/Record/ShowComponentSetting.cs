using Spine.Unity;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;


    [CreateAssetMenu]
    public class ShowComponentSetting : SerializedScriptableObject
    {
        [Tooltip("玩家名在UI中显示的颜色")]
        public Dictionary<string, Color> NameColors;
    }

