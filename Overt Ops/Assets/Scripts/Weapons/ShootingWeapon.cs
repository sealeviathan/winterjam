using UnityEngine;
[CreateAssetMenu(fileName = "New ShootingWeapon", menuName = "Items/Weapon Pieces/Shooting")]
public class ShootingWeapon : ScriptableObject
{
    //Specifically, shootable members
    [SerializeField]
    GameObject projectile;
    bool reloading;
    public bool canFire;
    [SerializeField]
    int curAmmo;
    [SerializeField]
    int clipSize;
    [SerializeField]
    int reservesAmmo;
    [SerializeField]
    int ammoConsumptionRate;
    [SerializeField]
    float fireCooldown;
    float curFireCooldown;
    [SerializeField]
    float reloadTime;
    float curReloadTime;
    [SerializeField]
    float effectiveDistance;
    bool projectileWeapon;

    public bool empty; //Use this bad boy if part is unused
    public ShootingWeapon()
    {
        empty = true;
    }
    public ShootingWeapon(GameObject projectile,int clipSize, int ammoConsumptionRate = 1, float fireCooldown = 0.25f,
    float reloadTime = 3f, float effectiveDistance = 50f)
    {
        this.projectileWeapon = false;
        empty = false;
        if(projectile != null)
        {
            this.projectileWeapon = true;
        }
        this.curReloadTime = 0;
        this.reloadTime = reloadTime;
        this.curAmmo = clipSize;
        this.clipSize = clipSize;
        this.curFireCooldown = 0;
        this.fireCooldown = fireCooldown;
        this.effectiveDistance = effectiveDistance;
    }
    public float _reloadTime
    {
        get {return this.reloadTime;}
    }
    public float _fireCooldown
    {
        get {return this.fireCooldown;}
    }

    public void UpdateShootableTimers(float timeScale)
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
            //Reload finished
            curAmmo = ReloadLogic();
        }

        if(curAmmo <= 0)
        {
            canFire = false;
        }
    }
    public void Reload()
    {
        if(curAmmo < clipSize && !reloading)
        {
            if(reservesAmmo > 0)
            {
                reloading = true;
                canFire = false;
                curReloadTime = reloadTime;
            }
        }
    }
    int ReloadLogic()
    {
        reloading = false;
        canFire = true;
        if(reservesAmmo >= clipSize)
        {
            reservesAmmo -= clipSize;
            return clipSize;
        }
        else
        {
            reservesAmmo = 0;
            return reservesAmmo;
        }
    }
    public void changeTimers(float newReloadTime, float newShotCooldownTime)
    {
        this.reloadTime = newReloadTime;
        this.fireCooldown = newShotCooldownTime;
    }
    public bool FireAmmo()
    {
        Debug.Log("Ammo fired");
        if(curAmmo - ammoConsumptionRate >= 0)
        {
            curFireCooldown = fireCooldown;
            curAmmo -= ammoConsumptionRate;
            return true;
        }
        return false;
    }
    public bool Bulletcast(Vector3 origin,Vector3 direction, out RaycastHit otherHit, LayerMask _playerMask)
    {
        otherHit = default(RaycastHit);
        
        if(Physics.Raycast(origin, direction,out otherHit, effectiveDistance, _playerMask))
        {
            return true;
        }
        return false;
    }
}
