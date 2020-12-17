using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Explosive
{
    public float fuseTime = 3f;
    bool dummy = false;

    private void Start()
    {
        health = 20;
        dummy = false;
        if(fuseTime == 0)
        {
            //dummy nade
            dummy = true;
        }
    }
    void Update()
    {
        if(!dummy)
        {
            fuseTime-=Time.deltaTime;
            if(fuseTime < 0)
            {
                Debug.Log("BOOM");
                Explode();
            }
        }
    }
}
