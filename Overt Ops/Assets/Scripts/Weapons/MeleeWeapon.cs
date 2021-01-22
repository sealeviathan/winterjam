using UnityEngine;
[CreateAssetMenu(fileName = "New MeleeWeapon", menuName = "Items/Weapon Pieces/Melee")]
public class MeleeWeapon : ScriptableObject
{
    //Specifically, melee members
    public bool canMelee;
    [SerializeField]
    float meleeRadius;
    [SerializeField]
    float meleeCooldown;
    float curMeleeCooldown;
    [SerializeField]
    float effectiveDistance;
    [SerializeField]
    float wackPower;
    public float _meleeCooldown
    {
        get {return this.meleeCooldown;}
    }
    public float _wackPower
    {
        get{return this.wackPower;}
    }

    public bool empty;
    public MeleeWeapon()
    {
        empty = true;
    }
    public MeleeWeapon(float meleeRadius, float meleeCooldown, float effectiveDistance, float wackPower = 5f)
    {
        this.meleeRadius = meleeRadius;
        this.meleeCooldown = meleeCooldown;
        this.curMeleeCooldown = 0;
        this.effectiveDistance = effectiveDistance;
        this.wackPower = wackPower;
    }

    public void UpdateMeleeTimers(float timeScale)
    {
        if(curMeleeCooldown > 0)
        {
            canMelee = false;
            curMeleeCooldown-= timeScale;
        }
        else
        {
            canMelee = true;
        }
    }
    public void changeTimers(float newMeleeCooldownTime)
    {
        this.meleeCooldown = newMeleeCooldownTime;
    }
    public bool Meleecast(Vector3 origin,Vector3 direction,out RaycastHit meleeHit, LayerMask _playerMask)
    {
        meleeHit = default(RaycastHit);
        this.curMeleeCooldown = meleeCooldown;
        if(Physics.SphereCast(origin,meleeRadius,direction,out meleeHit, effectiveDistance, _playerMask))
        {
            return true;
        }
        return false;
    }
}
