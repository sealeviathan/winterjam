using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseEntity
{
    // Start is called before the first frame update
    float speed = 10f;
    public Vector3 target;
    public Vector3[] waypoints;
    int curIter = 0;
    public GameObject player;
    UnityEngine.AI.NavMeshAgent agent;
    int damage = 5;

    void Start()
    {
        health = 10;
        target = waypoints[0];
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Search(waypoints);
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Player")
        {
            other.transform.GetComponent<Player>().Damage(damage);
        }
    }
    //Pretty simple, makes the transform point towards the target, then moves it's position towards it.
    //Following is a bad implementation, whoever is working on Enemies can either scrap this script or
    //Redesign it with rigidbodies in mind.
    private void MoveToTarget(Vector3 target)
    {
        agent.destination = target;
        agent.speed = speed;
    }
    
    //Search() takes an array of vector3 coordinates, cycles through them, goes from one to the next, then back to the 
    //first point when done cycling. If the player gets near enough, will initiate a chase.
    //when player leaves that range, goes back to roam.
    private void Search(Vector3[] points)
    {
        Vector3 playerPos;
        if(player != null)
        {
            playerPos = player.transform.position;
        }
        else
        {
            playerPos = new Vector3(9999, 9999, 9999);
        }
        float distance = AbsDistance(playerPos, transform.position);
        if(distance < 10)
        {
            target = player.transform.position;
        }
        else
        {
            target = waypoints[curIter];
            if(AbsDistance(target, transform.position) < 1)
            {
                curIter++;
                if(curIter > waypoints.Length - 1)
                {
                    curIter = 0;
                }
            }
        }
        MoveToTarget(target);

    }
    //Just math really, gets the absolute distance between two points. 
    float AbsDistance(Vector3 pos2, Vector3 pos1)
    {
        float xDis = pos2.x - pos1.x;
        float yDis = pos2.y - pos1.y;
        float zDis = pos2.z - pos1.z;
        return Mathf.Abs(Mathf.Sqrt((xDis * xDis) + (yDis * yDis) + (zDis * zDis)));
    }
}
