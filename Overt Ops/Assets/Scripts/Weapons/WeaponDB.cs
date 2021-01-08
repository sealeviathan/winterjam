using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDB : MonoBehaviour
{
    private static WeaponDB _instance;

    public static WeaponDB Instance { get { return _instance; } }
    ShootingWeapon basePistol = new ShootingWeapon(null,12);
    public Weapon deagle;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
    private void Start() {
        deagle = new Weapon(5, new LayerMask(), basePistol, default(ThrowingWeapon), default(MeleeWeapon));
    }
}
