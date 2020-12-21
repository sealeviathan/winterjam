using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 10f;
    public Vector3 target;
    public Vector3[] waypoints;
    int curIter = 0;
    public GameObject player;

    int damage = 5;
    int health = 10;

    void Start()
    {
        target = waypoints[0];
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
        transform.LookAt(target, Vector3.up);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    
    //Search() takes an array of vector3 coordinates, cycles through them, goes from one to the next, then back to the 
    //first point when done cycling. If the player gets near enough, will initiate a chase.
    //when player leaves that range, goes back to roam.
    private void Search(Vector3[] points)
    {
        float playerX;
        float playerZ;
        if(player != null)
        {
            playerX = player.transform.position.x;
            playerZ = player.transform.position.z;
        }
        else
        {
            playerX = 9999;
            playerZ = 9999;
        }
        float curX = transform.position.x;
        float curZ = transform.position.z;
        float distance = AbsDistance(playerX, curX, playerZ, curZ);
        if(distance < 10)
        {
            target = player.transform.position;
        }
        else
        {
            target = waypoints[curIter];
            if(AbsDistance(target.x, curX, target.z, curZ) < 1)
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
    float AbsDistance(float x2, float x1, float y2, float y1)
    {
        float xDis = x2 - x1;
        float yDis = y2 - y1;
        return Mathf.Abs(Mathf.Sqrt((xDis * xDis) + (yDis * yDis)));
    }
}
