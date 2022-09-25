using UnityEngine;

/// <summary>
/// 国外论坛看到的包装器，用于数组，是很好用
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class ExposedReferenceHolder<T> where T : Object
{
    public ExposedReference<T> ExposedReference;
}

