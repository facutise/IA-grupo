using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lucas Peck

public class Boid : SteeringAgent
{
    [SerializeField] Transform _seekTarget, _fleeTarget;
    [SerializeField, Range(0f, 2.5f)] float _alignmentWeight = 1;
    [SerializeField, Range(0f, 2.5f)] float _separationWeight = 1;
    [SerializeField, Range(0f, 2.5f)] float _cohesionWeight = 1;

    private void Start()
    {
        float x = Random.Range(-1f,1f);
        float y = Random.Range(-1f, 1f);

        var dir = new Vector3(x, y);

        _velocity = dir.normalized * _maxSpeed;

        GameManager.instance.allAgents.Add(this);
    }

    void Update()
    {
        Move();

        UpdateBoundPosition();

        if (Vector3.Distance(transform.position, _fleeTarget.position) <= _viewRadius)
        {
            AddForce(Flee(_fleeTarget.position));
        }
        else
        {
            AddForce(Arrive(_seekTarget.position));
        }

        transform.position += _velocity * Time.deltaTime;
        if (_velocity != Vector3.zero) transform.right = _velocity;

        Flocking();
    }

    private void Flocking()
    {
        var boids = GameManager.instance.allAgents;
        AddForce(Alignment(boids) * _alignmentWeight);
        AddForce(Separation(boids) * _separationWeight); //Se aplique un radio mas chico al actual
        AddForce(Cohesion(boids) * _cohesionWeight);
    }

    private void UpdateBoundPosition()
    {
        transform.position = GameManager.instance.AdjustPostionToBounds(transform.position);
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);
    }
}
