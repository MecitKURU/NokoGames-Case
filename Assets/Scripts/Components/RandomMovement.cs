using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RandomMovement : MonoBehaviour
{
    public Transform CenterPoint;
    public NavMeshAgent Agent;
    public Animator Animator;
    public float Range; //radius of sphere

    void Update()
    {
        if (Agent == null) return;

        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            Vector3 point;
            if (RandomPoint(CenterPoint.position, Range, out point))
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                Agent.SetDestination(point);
                Animator.SetFloat("Movement", Agent.speed);
            }
        }
    }

    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CenterPoint.position, Range);
    }
}