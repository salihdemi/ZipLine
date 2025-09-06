using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IState
{
    void EnterState(Player player);
    void UpdateState(Player player);
    void ExitState(Player player);
}
