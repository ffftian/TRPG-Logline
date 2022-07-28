using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;
namespace Miao
{
    public class FadeBehaviour<TValue, TComponent> : PlayableBehaviour where TComponent : class
    {
        public ProcessType processType;
        private TValue baseT;
        private TComponent component;

#if UNITY_EDITOR
        bool frist = true;
#endif
    }
}

