using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour, IDamageable, IKillable, IStabbable, IStaggerable
{
    public int health {get; set;}
    public int maxHealth {get; set;}

    public void Damage(int amount)
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
        other.transform.rotation = otherDirection;
    }

    public virtual void Stagger(float seconds)
    {

    }
    public virtual void FallDown()
    {

    }
}
