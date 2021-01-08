using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInventory
{
    Weapon[] weapons;
    bool updateTakeover;
    int curWepIndex;
    public int _curWepIndex
    {
        get{return curWepIndex;}
    }
    public void UpdateAvaliable()
    {
        if(!updateTakeover)
            weapons = regenerateInventory();
        updateTakeover = false;
    }
    Weapon[] regenerateInventory(Weapon additionalWep = default(Weapon))
    {
        Weapon[] curWeapons = weapons;
        Weapon[] newWeapons;
        int maxLength = curWeapons.Length;
        int count = 0;
        bool newWep = false;
        if(additionalWep != default(Weapon))
        {
            count++;
            newWep = true;
        }
        for(int i = 0; i < maxLength;i++)
        {
            if(curWeapons[i].isAvaliable)
            {
                count++;
            }
        }
        if(count == maxLength)
            return curWeapons;

        int avaliableCount = 0;
        newWeapons = new Weapon[count];
        
        for(int i = 0; i < maxLength; i++)
        {
            if(newWep && i == 0)
            {
                newWeapons[0] = additionalWep;
                additionalWep = default(Weapon);
            }
            else if(curWeapons[i].isAvaliable)
            {
                avaliableCount++;
                newWeapons[i] = curWeapons[i];
            }
        }
    
        return newWeapons;
    }
    public void AddWeapon(Weapon weapon)
    {
        regenerateInventory(weapon);
        updateTakeover = true;
    }
    public Weapon getCurrentWeapon()
    {
        return weapons[curWepIndex];
    }
    public Weapon getWeaponAtIndex(int i)
    {
        return weapons[i];
    }
    
}
