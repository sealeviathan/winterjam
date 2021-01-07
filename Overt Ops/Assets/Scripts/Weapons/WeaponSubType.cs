using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WeaponSubType
{
    public bool shootable;
    public bool throwable;
    public bool hitWithable;
    public bool autoFire;
    public bool canFire;
    public bool canMelee;
    public bool reloading;
    public bool isAvaliable;
    public int baseDamage;
    public int maxAmmo;
    public int curAmmo;
    public int ammoConsumptionRate;
    public int maxEffectRange;
    public float fireCooldown;
    public float curFireCooldown;
    public float reloadTime;
    public float curReloadTime;
    public float meleeCooldown;
    public float curMeleeCooldown;
    public float stunTime;
    public float meleeRadius;
    public float thrownVelocityMult;
    public float thrownArcMult;
    public LayerMask _playerMask;
    public Vector3 attackOrigin;
    public Vector3 attackDirection;
    public GameObject thrownObject;
    public GameObject projectile;
    public WeaponSubType(bool shootable, bool throwable, bool hitWithable, bool autoFire, int baseDamage, int maxAmmo, int maxEffectRange, float fireCooldown, float reloadTime, 
    float meleeCooldown, float stunTime, float meleeRadius, float thrownVelocityMult, float thrownArcMult, LayerMask _playerMask, GameObject thrownObject = null, GameObject projectile = null, int ammoConsumptionRate = 1)
    {
        this.baseDamage = baseDamage;
        this.curAmmo = maxAmmo;
        this.maxAmmo = maxAmmo;
        this.ammoConsumptionRate = ammoConsumptionRate;
        this.maxEffectRange = maxEffectRange;
        this.fireCooldown = fireCooldown;
        this.curFireCooldown = 0;
        this.reloadTime = reloadTime;
        this.curReloadTime = 0;
        this.meleeCooldown = meleeCooldown;
        this.curMeleeCooldown = 0;
        this.stunTime = stunTime;
        this.meleeRadius = meleeRadius;
        this.thrownVelocityMult = thrownVelocityMult;
        this.thrownArcMult = thrownArcMult;
        this.canFire = true;
        this.canMelee = true;
        this.autoFire = autoFire;
        this.reloading = false;

        this.shootable = shootable;
        this.throwable = throwable;
        this.hitWithable = hitWithable;

        this.isAvaliable = false;
        this.attackOrigin = new Vector3(); //Needs to be set every frame while active
        this.attackDirection = new Vector3(); //Same ^
        this.thrownObject = thrownObject;
        this.projectile = projectile;
        this._playerMask = _playerMask;
    }
    
}