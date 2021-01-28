using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInit : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] weaponPickups;
    void Start()
    {
        //This works, but it deletes the original prefab's components once the game ends, clearly a problem.
        //Make a clone of each weapon pickup weapon components, and set that as its new component.
        //Actually, need to make a const temp for each weapon, and then change the usable. Never 
        //Change the constant.
        
        for(int i = 0; i < weaponPickups.Length; i++)
        {
            PickupItem curPickup = weaponPickups[i].GetComponent<PickupItem>();
            Weapon curClone = curPickup.GetCloneWeapon();
            Weapon curConst = curPickup.GetConstWeapon();
            ShootingWeapon _shooting;
            _shooting = curConst.CloneShootingComponent();
            curClone.SetComponent<ShootingWeapon>(_shooting);
        }
    }
}
