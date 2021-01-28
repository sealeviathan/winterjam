using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Items/Weapon")]
public class Weapon : ScriptableObject
{
    //Mutual members-------------------------------------
    [SerializeField]
    int baseDamage;
    [SerializeField]
    public LayerMask _playerMask;
    public Vector3 attackOrigin = new Vector3(); //Needs to be set every frame while active//All types
    public Vector3 attackDirection = new Vector3(); //Same ^//All types
    public bool isAvaliable = false;
    [SerializeField]
    bool autoFire;
    [SerializeField]
    float stunTime;
    //A weapon is nothing without its mod classes
    [SerializeField]
    ShootingWeapon _shootingWeapon;
    [SerializeField]
    ThrowingWeapon _throwingWeapon;
    [SerializeField]
    MeleeWeapon _meleeWeapon;
    [SerializeField]
    bool shootable = false;
    [SerializeField]
    bool hitWithable = false;
    [SerializeField]
    bool throwable = false;
    public WeaponAppearance wepVisual;


    public Weapon(int baseDamage, LayerMask _playerMask, ShootingWeapon shootingWeapon, ThrowingWeapon throwingWeapon, 
    MeleeWeapon meleeWeapon, float stunTime = 0.25f, bool autoFire = false)
    {
        if(!shootingWeapon.empty && shootingWeapon != null)
        {
            shootable = true;
            _shootingWeapon = shootingWeapon;
        }
        if(!throwingWeapon.empty && throwingWeapon != null)
        {
            throwable = true;
            _throwingWeapon = throwingWeapon;
        }
        if(!meleeWeapon.empty && meleeWeapon != null)
        {
            hitWithable = true;
            _meleeWeapon = meleeWeapon;
        }
        this.baseDamage = baseDamage;
        this._playerMask = _playerMask;
        this.stunTime = stunTime;
        this.autoFire = autoFire;
        this.isAvaliable = false;
        Debug.Log("Constructor runs");
    }

    public ShootingWeapon CloneShootingComponent()
    {
        ShootingWeapon toReturn = Instantiate(_shootingWeapon) as ShootingWeapon;
        Debug.Log(toReturn);
        return toReturn;
    }

    public void SetComponent<T> (T compType)
    {
        if(compType.GetType() == typeof(ShootingWeapon))
        {
            _shootingWeapon = compType as ShootingWeapon;
        }
    }

    public void UpdateTimers(float timeScale)
    {
        if(shootable)
            _shootingWeapon.UpdateShootableTimers(timeScale);
        if(hitWithable)
            _meleeWeapon.UpdateMeleeTimers(timeScale);
        if(throwable)
            _throwingWeapon.UpdateThrowingTimers(timeScale);
    }
    ///<summary>The Abstract override class. Do not use as input, use PrimaryFire instead</summary>
    public virtual void PrimaryAttack()
    {
        if(_shootingWeapon.FireAmmo())
        {
            Debug.Log("Shooted");
            RaycastHit hit;
            bool reachedTarget = _shootingWeapon.Bulletcast(attackOrigin, attackDirection, out hit, _playerMask);
            if(reachedTarget)
            {
                GameObject hitObject = hit.transform.gameObject;
                DecalManager.decalManager.SpawnDecal(hit.point, -hit.normal);
                Debug.Log(hit.transform.gameObject);
                ApplyDamageInterfaces(hitObject);
            }
            //Do stuff to target; Implement the interfaces
        }
        return;
    }
    ///<summary>The Abstract override class. Do not use as input, use MeleeFire instead</summary>
    public virtual void MeleeAttack()
    {
        RaycastHit meleeHit;
        bool reachedTarget = _meleeWeapon.Meleecast(attackOrigin, attackDirection, out meleeHit, _playerMask);
        if(reachedTarget)
        {
            GameObject hitObject = meleeHit.transform.gameObject;
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
    ///<summary>The Abstract override class. Do not use as input, use ThrowFire instead</summary>
    public virtual void ThrowAttack()
    {
        _throwingWeapon.Throw(attackOrigin, attackDirection, out isAvaliable);
        return;
    }
    //Inputs
    //This section focuses on the unchangeable input handling
    public void PrimaryFire()
    {
        Debug.Log("PrimaryFire");
        if(shootable)
        {
            Debug.Log("Should Fire");
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
    public void Reload()
    {
        if(shootable)
        {
            _shootingWeapon.Reload();
        }
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
    public bool _autoFire
    {
        get{return this.autoFire;}
    }
}
