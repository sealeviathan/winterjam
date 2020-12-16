using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float fuseTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fuseTime-=Time.deltaTime;
        if(fuseTime < 0)
        {
            Debug.Log("BOOM");
            Explode();
        }
    }
    void Explode()
    {
        
        Destroy(gameObject);
    }
}
