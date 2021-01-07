using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Weapon : MonoBehaviour
{
    int baseDamage;
    int curAmmo;
    int maxAmmo;
    int ammoConsumptionRate;
    int maxEffectRange;
    float fireCooldown;
    float curFireCooldown = 0;
    float reloadTime;
    float curReloadTime = 0;
    float meleeCooldown;
    float curMeleeCooldown;
    float stunTime;
    float meleeRadius;
    float thrownVelocityMult;
    float thrownArcMult;
    bool canFire = true;
    bool canMelee = true;
    bool autoFire = false;
    bool reloading = false;

    bool shootable;
    bool throwable;
    bool hitWithable;
    public bool isAvaliable;
    public Vector3 attackOrigin;
    public Vector3 attackDirection;
    public GameObject thrownObject = null;
    public GameObject projectile = null;
    public LayerMask _playerMask;

    public Weapon(WeaponSubType weaponSubType)
    {
        this.baseDamage = weaponSubType.baseDamage;
        this.curAmmo = weaponSubType.maxAmmo;
        this.maxAmmo = weaponSubType.maxAmmo;
        this.ammoConsumptionRate = weaponSubType.ammoConsumptionRate;
        this.maxEffectRange = weaponSubType.maxEffectRange;
        this.fireCooldown = weaponSubType.fireCooldown;
        this.curFireCooldown = 0;
        this.reloadTime = weaponSubType.reloadTime;
        this.curReloadTime = 0;
        this.meleeCooldown = weaponSubType.meleeCooldown;
        this.curMeleeCooldown = 0;
        this.stunTime = weaponSubType.stunTime;
        this.meleeRadius = weaponSubType.meleeRadius;
        this.thrownVelocityMult = weaponSubType.thrownVelocityMult;
        this.thrownArcMult = weaponSubType.thrownArcMult;
        this.canFire = true;
        this.canMelee = true;
        this.autoFire = weaponSubType.autoFire;
        this.reloading = false;

        this.shootable = weaponSubType.shootable;
        this.throwable = weaponSubType.throwable;
        this.hitWithable = weaponSubType.hitWithable;

        this.isAvaliable = false;
        this.attackOrigin = new Vector3(); //Needs to be set every frame while active
        this.attackDirection = new Vector3(); //Same ^
        this.thrownObject = weaponSubType.thrownObject;
        this.projectile = weaponSubType.projectile;
        this._playerMask = weaponSubType._playerMask;
    }

    public void UpdateTimers(float timeScale)
    {
        if(curFireCooldown > 0)
        {
            canFire = false;
            curFireCooldown-= timeScale;
        }
        else
        {
            canFire = true;
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

        if(curMeleeCooldown > 0)
        {
            canMelee = false;
            curMeleeCooldown-= timeScale;
        }
        else
        {
            canMelee = true;
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
            //Do stuff to target; Implement the interfaces
        }
        return;
    }
    public virtual void MeleeAttack()
    {
        RaycastHit meleeHit;
        bool reachedTarget = Meleecast(attackOrigin, attackDirection, out meleeHit);
        //Do stuff to target
        return;
    }
    public virtual void ThrowAttack()
    {
        Throw(attackOrigin, attackDirection, thrownVelocityMult, thrownArcMult);
        return;
    }
    //Inputs
    //This section focuses on the unchangeable input handling
    public void PrimaryFire()
    {
        if(shootable)
        {
            if(canFire)
                PrimaryAttack();
        }
        else if(hitWithable && !shootable && !throwable)
        {
            if(canMelee)
                MeleeAttack();
        }
        else if(throwable && !shootable && !hitWithable)
        {
            ThrowAttack();
        }
    }
    public void MeleeFire()
    {
        if(canMelee)
        {
            if(hitWithable)
                MeleeAttack();
        }
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
    //End inputs
    bool FireAmmo(int amount)
    {
        if(curAmmo - amount >= 0)
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
    public bool Meleecast(Vector3 origin,Vector3 direction,out RaycastHit meleeHit)
    {
        meleeHit = default(RaycastHit);
        if(hitWithable)
        {
            if(Physics.SphereCast(origin,meleeRadius,direction,out meleeHit, maxEffectRange, _playerMask))
            {
                return true;
            }
            return false;
        }
        else
        {
            Debug.LogWarning("You are trying to hit with an un-hitWithable weapon. Embarrasing.");
            return false;
        }
    }
    public void Throw(Vector3 origin, Vector3 direction, float velocityMult, float arcMult)
    {
        if(throwable)
        {
            //The idea is to add a vector3.up multiplied by arcmult to the direction times the velocity
            //mult, this way, instead of throwing in a boring straight line, the item is tossed in
            //more of an arc fashion. Or, similarly, if you want no arc, multiply the arc by 0,
            //and it will throw straight.
            if(thrownObject != null)
            {
                isAvaliable = false;
                Vector3 throwVector = direction * velocityMult + Vector3.up * arcMult;
            }
            else
            {
                Debug.LogError("Trying to throw a weapon with no thrown object item attached!");
            }
        }
        else
        {
            Debug.LogWarning("You are trying to throw an unthrowable weapon. Not necessarily impossible, just ineffective.");
        }
    }
    public void setAutoFire(bool boolean)
    {
        this.autoFire = boolean;
    }
    public void changeTimers(float newReloadTime, float newShotCooldownTime, float newMeleeCooldownTime)
    {
        this.reloadTime = newReloadTime;
        this.fireCooldown = newShotCooldownTime;
        this.meleeCooldown = newMeleeCooldownTime;
    }
    public float _reloadTime
    {
        get {return this.reloadTime;}
    }
    public float _fireCooldown
    {
        get {return this.fireCooldown;}
    }
    public float _meleeCooldown
    {
        get {return this.fireCooldown;}
    }
}
