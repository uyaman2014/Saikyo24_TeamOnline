using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> where T : class, new()
{
    // ����A�O����R���X�g���N�^���Ă΂ꂽ�Ƃ��ɁA�����ň����|����
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