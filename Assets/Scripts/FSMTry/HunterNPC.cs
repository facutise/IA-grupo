using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Facundo Sebastian Tisera
public class HunterNPC : MonoBehaviour//Player
{
    MyHunterFiniteStateMachine _fsm; //Con esto lo estamos guardando en nuestra variable
    public Transform[] _wayPointsArray;
    public LayerMask _obstacles;
    public LayerMask _enemies;
    public GameObject[] _arrayOfEnemies;
    //public EnergyBar EnergyBarScript;
    void Start()
    {
        _fsm = new MyHunterFiniteStateMachine(); //Esto esta creando una nueva FSM;
        _fsm.AddState(PlayerStates.Idle, new HunterPatrolState(this));
        _fsm.AddState(PlayerStates.Move, new Hunter(this));
        //_fsm.AddState(PlayerStates.Dying, new DyingState(this));
        //_fsm.ChangeState(PlayerStates.Idle);
        _fsm.ChangeState(PlayerStates.Idle);

    }
    public void Update()
    {
        _fsm.Update();
    }
}

    public enum PlayerStates
    {
        Idle,
        Move,
        
    }