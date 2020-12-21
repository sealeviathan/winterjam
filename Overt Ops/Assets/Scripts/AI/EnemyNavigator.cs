using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNavigator : MonoBehaviour
{
    public Transform goal;
    UnityEngine.AI.NavMeshAgent agent;
       
    void Start ()
    {
       agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
       
    }
    private void Update()
    {
        agent.destination = goal.position;
       
    }
}
