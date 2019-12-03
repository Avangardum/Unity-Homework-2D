using System;
using UnityEngine;

public class SingletonMonoBehavoiur<T> : MonoBehaviour where T : SingletonMonoBehavoiur<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = (T)this;
        else
            throw new Exception($"Создано более одного экземпляра {typeof(T)}");
    }
}
