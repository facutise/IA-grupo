using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateFather//=state
{
    public abstract void OnEnter();

    public abstract void ThisStateUpdate();
    public abstract void OnExit();

    public MyHunterFiniteStateMachine HunterFSM;

}
