using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;

public class GameManager : Singletone<GameManager>
{
    private void Awake() => Init();
    private void Init()
    {
        base.SingletoneInit();
    }
}
