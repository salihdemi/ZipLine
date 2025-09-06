using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : MonoBehaviour, IState
{
    public void EnterState(Player player)
    {
        throw new NotImplementedException();
    }

    public void ExitState(Player player)
    {
        throw new NotImplementedException();
    }

    public void UpdateState(Player player)
    {
        player.Move();
        player.CheckJump();
        player.Turn();
    }
}
