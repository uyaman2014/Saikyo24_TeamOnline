using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> where T : class, new()
{
    // 万一、外からコンストラクタを呼ばれたときに、ここで引っ掛ける
    protected Singleton()
    {
        Debug.Assert(null == _instance);
    }
    private static readonly T _instance = new T();

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
}