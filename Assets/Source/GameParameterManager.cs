using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameParameterManager : Singleton<GameParameterManager>
{
    public float TimeLimit { get; set; } = 20;
    public int TargetClickCount { get; set; } = 100;
}
