using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IExplodeable
{
    
    public int radius {get; set;}
    public int damage {get; set;}
    public float fuseTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        radius = 3;
        damage = 25;
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
    public void Explode()
    {
        DamageInArea(radius, damage);
        Destroy(gameObject);
    }
    public void DamageInArea(int _radius, int _damage)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);
        foreach(Collider hit in hits)
        {
            
            if()
            {
                Debug.Log(" there is a Damageable");

                damaged.Damage(_damage);
                Debug.Log(damaged.health);

            }
        }
    }
}
