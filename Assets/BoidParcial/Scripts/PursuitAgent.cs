using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitAgent : SteeringAgent
{

    //Queremos perseguir al agente que esta persiguiendo al Seek con Obstacle Avoidance

    //[SerializeField] Transform _target;
    [SerializeField] SteeringAgent _target;
    [SerializeField] float _rangeToKill;

    // Update is called once per frame
    void Update()
    {
        if (!HastToUseObstacleAvoidance()) AddForce(Pursuit(_target));
        Move();

        KillMyTarget();
    }

    private void KillMyTarget()
    {
        if (Vector3.Distance(transform.position, _target.transform.position) <= _rangeToKill) _target.ResetPosition();
    }
}
