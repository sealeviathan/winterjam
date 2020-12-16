using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IExplodeable
{
    
    public int radius {get; set;}
    public int damage {get; set;}
    public float force {get; set;}
    public float fuseTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        radius = 6;
        damage = 25;
        force = 350f;
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
        DamageInArea(radius, damage, force);
        Destroy(gameObject);
    }
    public void Kill()
    {
        Explode();
    }
    public void DamageInArea(int _radius, int _damage, float _force)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radius);
        foreach(Collider hit in hits)
        {
            if(!hit.gameObject.isStatic)
            {
                Rigidbody hitRB = hit.gameObject.GetComponent<Rigidbody>();
                if(hitRB != null)
                {
                    hitRB.AddExplosionForce(_force,transform.position,_radius);
                    hitRB.AddExplosionForce(0,transform.position,_radius,_force);
                }
                IDamageable damaged = hit.gameObject.GetComponent<IDamageable>();
                if(damaged != null)
                {
                    damaged.Damage(_damage);
                }
            }
        }
    }
}
