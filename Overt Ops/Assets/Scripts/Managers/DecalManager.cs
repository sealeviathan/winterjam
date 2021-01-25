using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalManager : MonoBehaviour
{
    public static DecalManager decalManager;
    public Queue<GameObject> decalPool;
    public GameObject poolObject;
    public int poolCount = 50;
    private void Awake()
    {
        decalManager = this;
    }
    private void Start()
    {
        GameObject temp;
        decalPool = new Queue<GameObject>();
        for(int i = 0; i < poolCount; i++)
        {
            temp = Instantiate(poolObject);
            temp.SetActive(false);
            decalPool.Enqueue(temp);
        }
    }
    public void SpawnDecal(Vector3 pos, Vector3 direction)
    {
        GameObject poolObj = decalPool.Dequeue();
        poolObj.transform.position = pos;
        poolObj.transform.forward = direction;
        //poolObj.transform.position += transform.forward; 
        poolObj.SetActive(true);
        decalPool.Enqueue(poolObj);
    }
}
