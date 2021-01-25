using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour, IDamageable, IKillable, IStabbable, IStaggerable, IWackable
{
    
    public int health {get; set;}
    public int maxHealth {get; set;}

    public virtual void Damage(int amount)
    {
        //A publicly accessible damage method
        health -= amount;
        if(health < 0)
        {
            Kill();
        }
    }
    public virtual void Kill()
    {
        gameObject.SetActive(false);
    }

    ///<summary>Stick GameObject 'other' with 'otherDirection' into current gameObject.
    ///Then, set 'other' as a child of this gameobject.</summary>
    public void GetStabbed(GameObject other, Vector3 otherDirection)
    {
        other.transform.localEulerAngles = otherDirection;
        other.transform.SetParent(gameObject.transform);
    }

    public virtual void Stagger(float seconds)
    {

    }
    public virtual void FallDown()
    {

    }
    public virtual void Wack(Vector3 direction, float force)
    {
        Rigidbody selfRB = transform.GetComponent<Rigidbody>();
        if(selfRB != null)
        {
            selfRB.velocity = direction * force;
        }
    }
}
