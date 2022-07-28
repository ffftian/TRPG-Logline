using Spine.Unity;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;


    [CreateAssetMenu]
    public class ShowComponentSetting : SerializedScriptableObject
    {
        public Dictionary<string, Color> NameColors;
    }

