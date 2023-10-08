using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Para el sistema de energ�a hacer algo parecido a la bater�a en  Aplicaci�n de Motores HECHO*/

/*La vuelta al ser esquivado debe tener un poco de delay para que sea mas realista
 clase 5 tiene el obstacle avoidance, el pursuit y el range to kill (no necesariamente laburar con herencia o scripts como
 parametro, laburar con transforms es m�s f�cil*/

/*El pursuit funciona basicamente al hacer un seek pero el vector director es la suma de la posicion de la presa + el vector velocidad( de dicha presa, creo)*/

/*Re ver y analizar profundamente obstacle Avoidance que entend� pero me re perd�, ver primero el script*/

//FALTA PURSUIT-KILL- OBSTACLE AVOIDANCE- CAMBIO DE ESTADOS.
public class Hunter : StateFather//=moveState
{
    [SerializeField] Transform _target;
    [SerializeField] float _maxSpeed;
    [SerializeField] float _rangeToKill;
    [SerializeField] float _radius;
    [SerializeField] float _maxForce;
    //[SerializeField] Transform[] _wayPoints;
    [SerializeField] LayerMask _obstacles;
    [SerializeField] LayerMask _enemies;
    Vector3 _velocity;
    Vector3 _desired;

    [SerializeField] GameObject[] _arrayOfEnemies;
    [SerializeField] int _actualPrey;
    public Renderer _rend;
    public EnergyBar EnergyBarScript;

    public Transform HunterTransform;
    public Hunter(HunterNPC p)
    {
        _maxSpeed = 3;
        _rangeToKill = 1;
        _radius = 2;
        _maxForce = 2;
        _obstacles = p._obstacles;
        _enemies = p._enemies;
        _arrayOfEnemies = p._arrayOfEnemies;
        _actualPrey = 0;

        _actualPrey = 0;
        _rend = p.GetComponent<Renderer>();

        EnergyBarScript = p.EnergyBarScript;

        HunterTransform = p.transform;
    }

    private void Update()
    {
        ThisStateUpdate();
    }
    /*para el Pursuit en vez de usar un transform usamos como variable el script de la presa para acceder a su vector velocity, entonces,
      el vector3 desired pasar�a a ser el siguiente: (target.transform.position + target.velocity) - transform.position; */
    //para que no rote de una clampeamos el steering

    private void seek()
    {
        /*Vector3 desired = _target.position - transform.position;
        desired.Normalize();
        desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        AddForce(steering);*/

        //_desired = _target.position - transform.position;
        _desired.Normalize();
        _desired *= _maxSpeed;

        Vector3 steering = _desired - _velocity;
        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        AddForce(steering);

    }
    /*
    private void WayPointsSystem(int ActualWaypoint)
    {
        Vector3 desired = _wayPoints[ActualWaypoint].transform.position - transform.position;
        desired.Normalize();
        desired *= _maxSpeed;

        Vector3 steering = desired - _velocity;

        steering = Vector3.ClampMagnitude(steering, _maxForce * Time.deltaTime);

        AddForce(steering);


    }
    */
    private void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxSpeed);
    }

    /* Vector3 ObstacleAvoidance()
     {
         if (Physics.Raycast(transform.position + transform.up * 0.5f, transform.right, _viewRadius, _obstacles))
             return Seek(transform.position - transform.up);
         else if (Physics.Raycast(transform.position - transform.up * 0.5f, transform.right, _viewRadius, _obstacles))
             return Seek(transform.position + transform.up);
         return Vector3.zero;
     }
    */
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
            _desired = _arrayOfEnemies[_actualPrey].transform.position- HunterTransform.position;
        }
    }
    public void RangeToKill()
    {
        //new RaycastHit a = transform.position,transform.right, ; 
        //RaycastHit a;
        if (Physics.Raycast(HunterTransform.position, HunterTransform.right, _rangeToKill, _enemies) || Physics.Raycast(HunterTransform.position, -HunterTransform.right, _rangeToKill, _enemies) || Physics.Raycast(HunterTransform.position, HunterTransform.up, _rangeToKill, _enemies) || Physics.Raycast(HunterTransform.position, -HunterTransform.up, _rangeToKill, _enemies))
        {
            if (_arrayOfEnemies[_actualPrey].gameObject == null)
            {
                _actualPrey += 1;
                //Destroy(_arrayOfEnemies[_actualPrey].gameObject);
                _arrayOfEnemies[_actualPrey].gameObject.SetActive(false);
            }
            else
            {
               // Destroy(_arrayOfEnemies[_actualPrey].gameObject);
                _arrayOfEnemies[_actualPrey].gameObject.SetActive(false);
                _actualPrey += 1;

            }
            //Destroy(.collider.gameObject);
            /*if(a.(transform.position, transform.right, _rangeToKill, _enemies))
            {

            }*/
        }
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
        Debug.Log("Cazador entro a estado Move");
        _rend.material.color = Color.red;
    }

    
    public override void ThisStateUpdate()
    {
        ObstacleAvoidance();
        seek();


        HunterTransform.position += _velocity * Time.deltaTime;
        //para que mire al objetivo
        HunterTransform.right = _velocity;

        RangeToKill();

        EnergyBarScript.HuntingModeEnergyConsumption();//Funci�n que consume la energ�a

        if (EnergyBarScript.currentEnergyValue <= EnergyBarScript.MinEnergy)
        {
            

            HunterFSM.ChangeState(PlayerStates.Idle);
            Debug.Log("Se ha cambiado a estado Idle del cazador");

        }

    }

    public override void OnExit()
    {
        Debug.Log("Cazador salio del estado Move");
        _rend.material.color = Color.white;
    }
}
