using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Weapon : MonoBehaviour
{
    //Mutual members-------------------------------------
    int baseDamage;
    public LayerMask _playerMask;
    public Vector3 attackOrigin = new Vector3(); //Needs to be set every frame while active//All types
    public Vector3 attackDirection = new Vector3(); //Same ^//All types
    bool isAvaliable = false;
    bool autoFire;
    float stunTime;
    //A weapon is nothing without its mod classes
    ShootingWeapon _shootingWeapon;
    ThrowingWeapon _throwingWeapon;
    MeleeWeapon _meleeWeapon;

    bool shootable = false;
    bool hitWithable = false;
    bool throwable = false;


    public Weapon(int baseDamage, LayerMask _playerMask, ShootingWeapon shootingWeapon, ThrowingWeapon throwingWeapon, 
    MeleeWeapon meleeWeapon, float stunTime = 0.25f, bool autoFire = false)
    {
        if(!shootingWeapon.empty)
        {
            shootable = true;
            _shootingWeapon = shootingWeapon;
        }
        if(!throwingWeapon.empty)
        {
            throwable = true;
            _throwingWeapon = throwingWeapon;
        }
        if(!meleeWeapon.empty)
        {
            hitWithable = true;
            _meleeWeapon = meleeWeapon;
        }
        this.baseDamage = baseDamage;
        this._playerMask = _playerMask;
        this.stunTime = stunTime;
        this.autoFire = autoFire;
        this.isAvaliable = false;
    }

    public void UpdateTimers(float timeScale)
    {
        _shootingWeapon.UpdateShootableTimers(timeScale);
        _meleeWeapon.UpdateMeleeTimers(timeScale);
        _throwingWeapon.UpdateThrowingTimers(timeScale);
    }
    public virtual void PrimaryAttack()
    {
        if(_shootingWeapon.FireAmmo())
        {
            RaycastHit hit;
            bool reachedTarget = _shootingWeapon.Bulletcast(attackOrigin, attackDirection, out hit, _playerMask);
            if(reachedTarget)
            {
                GameObject hitObject = hit.transform.GetComponent<GameObject>();
                ApplyDamageInterfaces(hitObject);
            }
            //Do stuff to target; Implement the interfaces
        }
        return;
    }
    public virtual void MeleeAttack()
    {
        RaycastHit meleeHit;
        bool reachedTarget = _meleeWeapon.Meleecast(attackOrigin, attackDirection, out meleeHit, _playerMask);
        if(reachedTarget)
        {
            GameObject hitObject = meleeHit.transform.GetComponent<GameObject>();
            ApplyDamageInterfaces(hitObject);
            IWackable wackable = hitObject.GetComponent<IWackable>();
            if(wackable != null)
            {
                wackable.Wack(attackDirection, _meleeWeapon._wackPower);
            }

        }
        //Do stuff to target
        return;
    }
    public virtual void ThrowAttack()
    {
        _throwingWeapon.Throw(attackOrigin, attackDirection, out isAvaliable);
        return;
    }
    //Inputs
    //This section focuses on the unchangeable input handling
    public void PrimaryFire()
    {
        if(shootable)
        {
            if(_shootingWeapon.canFire)
                PrimaryAttack();
        }
        else if(hitWithable && !shootable)
        {
            if(_meleeWeapon.canMelee)
                MeleeAttack();
        }
        else if(throwable && !shootable && !hitWithable)
        {
            ThrowAttack();
        }
    }
    public void MeleeFire()
    {
        if(_meleeWeapon.canMelee)
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
    public void ApplyDamageInterfaces(GameObject hitObject)
    {
        IDamageable damageable = hitObject.GetComponent<IDamageable>();
        IStaggerable staggerable = hitObject.GetComponent<IStaggerable>();
        if(damageable != null)
        {
            damageable.Damage(baseDamage);
        }
        if(staggerable != null)
        {
            staggerable.Stagger(stunTime);
        }
    }
    //End inputs
    public void SetAutoFire(bool boolean)
    {
        this.autoFire = boolean;
    }
    public void SetAttackVectors(Vector3 newAttackOrigin, Vector3 newAttackDirection)
    {
        this.attackOrigin = newAttackOrigin;
        this.attackDirection = newAttackDirection;
    }
    public void SetBaseDamage(int newBaseDamage)
    {
        this.baseDamage = newBaseDamage;
    }
}
