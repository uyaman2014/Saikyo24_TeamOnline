using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public void Clicked()
    {
        var pInfos = NetworkManager.Instance.PlayerInfos;
        foreach (var pInfo in pInfos)
        {
            if(pInfo.Value.bIsSelf) 
                NetworkManager.Instance.SendClickedCount(pInfo.Value.Count + 1);
        }
    }
}
