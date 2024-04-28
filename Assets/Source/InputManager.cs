using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private int _clickedCount;
    public void Clicked()
    {
        _clickedCount++;
        NetworkManager.Instance.SendClickedCount(_clickedCount);
    }
    public void ResetCount()
    {
        _clickedCount = 0;
    }
}
