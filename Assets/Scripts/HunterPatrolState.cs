using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Facundo Sebastian Tisera
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

    [SerializeField] LayerMask _obstacles;
    Vector3 _desired;

    public float MaxEnergy = 1;
    public float MinEnergy = 0;
    public float DecreaseSpeedOfEnergy = 0.05f;

    public float TimeTakenForRecoveryEnergyValue;
    public HunterPatrolState(HunterNPC p)//constructor
    {
        _maxSpeed = 5;
        _radius = 2;
        _maxForce = 3;
        _wayPointsArray = p._wayPointsArray;
        _myWayPointInt = 0;
        _rend = p.GetComponent<Renderer>();

        EnergyBarScript = p.EnergyBarScript;

        HunterTransform = p.transform;
        _obstacles = p._obstacles;

        MaxEnergy = 1;
        MinEnergy = 0;
        DecreaseSpeedOfEnergy = 0.05f;
    }

    public void ObstacleAvoidance()
    {
        if (Physics.Raycast(HunterTransform.position + HunterTransform.up * 0.5f, HunterTransform.right, _radius, _obstacles))
        {
            _desired = HunterTransform.position - HunterTransform.up;

        }
        else if (Physics.Raycast(HunterTransform.position - HunterTransform.up * 0.5f, HunterTransform.right, _radius, _obstacles))
        {
            _desired = HunterTransform.position + HunterTransform.up;
        }
        else
        {
            _desired = _wayPointsArray[_myWayPointInt].transform.position - HunterTransform.position;
        }
    }
    public float TimeTakenToStartRecoveringEnergy()
    {
        TimeTakenForRecoveryEnergyValue -= DecreaseSpeedOfEnergy * Time.deltaTime;

        return TimeTakenForRecoveryEnergyValue;
    }


    public override void ThisStateUpdate()//"Update" de este script
    {
        if (TimeTakenToStartRecoveringEnergy() <= 0.1)
        {
            EnergyBarScript.EnergyRecoveryIdleStateFunction();//Función que recarga la energía 

        }


        if (EnergyBarScript.currentEnergyValue >= 0.9f)
        {


            HunterFSM.ChangeState(PlayerStates.Move);
            Debug.Log("Se ha cambiado a estado Move del cazador");

        }


        
        Vector3 ActualDir = _wayPointsArray[_myWayPointInt].position - HunterTransform.position;
        if (ActualDir.magnitude < _radius)//IF para pasar de un waypoint a otro
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
        _desired = _wayPointsArray[ActualWaypoint].transform.position - HunterTransform.position;
        _desired.Normalize();
        _desired *= _maxSpeed;

        Vector3 steering = _desired - _velocity;

        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        AddForce(steering);


        if (_desired.magnitude < _radius)//IF para pasar de un waypoint a otro
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
        TimeTakenForRecoveryEnergyValue = MaxEnergy;
    }


    public override void OnExit()
    {
        Debug.Log("sali de idle");
    }
}
