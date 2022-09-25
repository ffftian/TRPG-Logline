using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace MiaoTween
{
    [Serializable]
    public class TextMeshProChangeBehaviour: PlayableBehaviour
    {
        [SerializeField][TextArea] public string dialogue = "对话内容";
        private TMP_Text component;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            base.ProcessFrame(playable, info, playerData);
            if (component == null)
            {
                component = playerData as TMP_Text;
                component.text = dialogue;
            }
            double percentage = playable.GetTime() / playable.GetDuration();
            RefreshShow(percentage+0.2f);
        }
        public void RefreshShow(double percentage)
        {
            component.maxVisibleCharacters = (int)(component.text.Length * percentage);
        }
    }
}
