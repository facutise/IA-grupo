using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHunterFiniteStateMachine//=FiniteStateMachine
{

    StateFather _currentState;
    //Diccionario de estados
    Dictionary<PlayerStates, StateFather> _allStates = new Dictionary<PlayerStates, StateFather>();
    //Si este codigo quiero que sea generico, no deberia estar llamando a un enum en especifico

    public void Update()
    {
        //if(_currentState != null) _currentState.OnUpdate(); //Evita llamados nulos
       
        //Versiones mas recientes
        _currentState?.ThisStateUpdate(); //Evita llamados nulos
    }

    public void AddState(PlayerStates name, StateFather state)
    {
        if (!_allStates.ContainsKey(name))
        {
            _allStates.Add(name, state);
            state.HunterFSM = this;
        }
        else
        {
            _allStates[name] = state;
        }
    }

    public void ChangeState(PlayerStates name) //Para ir cambiando de estados, los vamos pidiendo
    {
        _currentState?.OnExit(); //Si o si necesario preguntar por null
        if (_allStates.ContainsKey(name)) _currentState = _allStates[name];
        _currentState?.OnEnter();
    }
   
}
