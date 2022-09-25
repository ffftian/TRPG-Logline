using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;

namespace PostProcess
{
    public abstract class PostProcessData<TData> : PlayableBehaviour where TData : new()
    {
        [SerializeField]
        public TData data;
    }
    public abstract class PostProcessBehaviour<TComponent, TData> : PostProcessData<TData> where TComponent : IPostProcessComponent where TData : new()
    {

        public abstract void Assign(TComponent postProcessComponent);

        public abstract void Obtain(TComponent postProcessComponent);

        public abstract void Enable(TComponent postProcessComponent);

        public abstract void Disable(TComponent postProcessComponent);

        public abstract void Plus(TData addData, float weight);

        public abstract void Clear();
    }

}
