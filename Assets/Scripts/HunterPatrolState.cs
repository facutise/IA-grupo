using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterPatrolState : StateFather//=IdleState
{
    [SerializeField] float _maxSpeed;
    [SerializeField] float _radius;
    [SerializeField] float _maxForce;
    [SerializeField] Transform[] _wayPointsArray;
    Vector3 _velocity;
    [SerializeField] int _myWayPointInt;
    Renderer _rend;
    public EnergyBar EnergyBarScript;

    public Transform HunterTransform;
    public HunterPatrolState(HunterNPC p)
    {
        _maxSpeed = 3;
        _radius = 2;
        _maxForce = 2;
        _wayPointsArray = p._wayPointsArray;
        _myWayPointInt = 0;
        _rend = p.GetComponent<Renderer>();

        EnergyBarScript = p.EnergyBarScript;

        HunterTransform = p.transform;


    }


   
    private void Update()
    {
        
        ThisStateUpdate();
    }
   
    
    public override void ThisStateUpdate()
    {
        EnergyBarScript.EnergyRecoveryIdleStateFunction();//Funci�n que recarga la energ�a 
        
        if (EnergyBarScript.currentEnergyValue >= EnergyBarScript.MinEnergy)
        {


            HunterFSM.ChangeState(PlayerStates.Move);
            Debug.Log("Se ha cambiado a estado Move del cazador");

        }



        Vector3 ActualDir = _wayPointsArray[_myWayPointInt].position - HunterTransform.position;
        if (ActualDir.magnitude < _radius)
        {
            if (_myWayPointInt >= _wayPointsArray.Length - 1)
            {
                _myWayPointInt = 0;
            }
            else
            {
                _myWayPointInt += 1;
            }
        }

        WayPointsSystem(_myWayPointInt);
        /*if(transform.position== _wayPointsArray[_myWayPointInt].transform.position)
        {
            if (_myWayPointInt >= _wayPointsArray.Length)
            {
                _myWayPointInt = 0;
            }
            else
            {
                _myWayPointInt += 1;
            }
        }*/

        HunterTransform.position += _velocity * Time.deltaTime;
        //para que mire al objetivo
        HunterTransform.right = _velocity;
    }

    private void WayPointsSystem(int ActualWaypoint)
    {
        Vector3 desired = _wayPointsArray[ActualWaypoint].transform.position - HunterTransform.position;
        desired.Normalize();
        desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;

        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        AddForce(steering);


    }

    private void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(HunterTransform.position, _radius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(HunterTransform.position + HunterTransform.up * 0.5f, HunterTransform.position + HunterTransform.up * 0.5f + HunterTransform.right * _radius);
        Gizmos.DrawLine(HunterTransform.position - HunterTransform.up * 0.5f, HunterTransform.position - HunterTransform.up * 0.5f + HunterTransform.right * _radius);
    }

    public override void OnEnter()
    {
        Debug.Log("entre a idle");
    }

   
    public override void OnExit()
    {
        Debug.Log("sali a idle");
    }
}