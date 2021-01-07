using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Weapon : MonoBehaviour
{
    [SerializeField]
    int baseDamage;
    [SerializeField]
    int curAmmo;
    [SerializeField]
    int maxAmmo {get;}
    int ammoConsumptionRate;
    [SerializeField]
    int maxEffectRange;
    [SerializeField]
    float fireCooldown {get;}
    float curCooldown = 0;
    [SerializeField]
    float reloadTime {get;}
    float curReloadTime = 0;
    [SerializeField]
    float stunTime;
    bool canFire = true;
    bool autoFire = false;
    bool reloading = false;

    bool shootable;
    bool throwable;
    bool hitWithable;
    public Vector3 attackOrigin;
    public Vector3 attackDirection;
    public GameObject thrownObject;
    public LayerMask _playerMask;

    public void UpdateTimers(float timeScale)
    {
        if(curCooldown > 0)
        {
            canFire = false;
            curCooldown-= timeScale;
        }

        if(curReloadTime > 0)
        {
            canFire = false;
            curReloadTime-= timeScale;
        }
        else if(curReloadTime <= 0 && reloading)
        {
            reloading = false;
            canFire = true;
            curAmmo = maxAmmo;
        }

        if(curAmmo <= 0)
        {
            canFire = false;
        }
    }
    public virtual void PrimaryAttack()
    {
        if(FireAmmo(ammoConsumptionRate))
        {
            RaycastHit hit;
            bool reachedTarget = Bulletcast(attackOrigin, attackDirection, out hit);
        }
        return;
    }
    public virtual void MeleeAttack()
    {
        return;
    }
    public virtual void ThrowAttack()
    {
        return;
    }
    //Inputs
    public void PrimaryFire()
    {
        if(shootable)
        {
            PrimaryAttack();
        }
        else if(hitWithable && !shootable && !throwable)
        {
            MeleeAttack();
        }
        else if(throwable && !shootable && !hitWithable)
        {
            ThrowAttack();
        }
    }
    public void MeleeFire()
    {
        if(hitWithable)
            MeleeAttack();
    }
    public void ThrowFire()
    {
        if(throwable)
            ThrowAttack();
    }
    public void Reload()
    {
        if(curAmmo < maxAmmo && !reloading)
        {
            reloading = true;
            canFire = false;
            curReloadTime = reloadTime;
        }
    }
    bool FireAmmo(int amount)
    {
        if(curAmmo - amount > 0)
        {
            curAmmo-= amount;
            return true;
        }
        return false;
    }
    ///<returns>A RaycastHit of whatever the raycast hit</returns>
    public bool Bulletcast(Vector3 origin,Vector3 direction, out RaycastHit otherHit)
    {
        otherHit = default(RaycastHit);
        if(shootable)
        {
            if(Physics.Raycast(origin, direction,out otherHit, maxEffectRange, _playerMask))
            {
                return true;
            }
            return false;
        }
        else
        {
            Debug.LogWarning("You are trying to shoot an unshootable weapon, why??");
            return false;
        }
    }
    ///<returns>A Collider array from a spherecast, indicating said hit area</returns>
    public bool Meleecast(Vector3 origin,Vector3 direction, float hitRadius, out Collider[] meleeHits)
    {
        meleeHits = default(Collider[]);
        if(hitWithable)
        {

        }
        else
        {
            Debug.LogWarning("You are trying to hit with an un-hitWithable weapon. Embarrasing.");
            return false;
        }
    }
    public bool Throwcast(Vector3 origin, Vector3 direction, out RaycastHit hitOther)
    {
        hitOther = default(RaycastHit);
        if(throwable)
        {

        }
        else
        {
            Debug.LogWarning("You are trying to throw an unthrowable weapon. Not necessarily impossible, just ineffective.");
            return false;
        }
    }

}
