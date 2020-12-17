using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour, IDamageable, IKillable
{
    public int health {get; set;}
    public int maxHealth {get; set;}
    public int radius = 6;
    public int damage = 25;
    public float force = 350f;
    
    
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
    public void Damage(int _amount)
    {
        health -= _amount;
        if(health < 0)
        {
            Kill();
        }
    }
}
