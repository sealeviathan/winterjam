using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInventory
{
    List<Weapon> weapons;
    int curWepIndex;
    public PlayerInventory(List<Weapon> curWeapons)
    {
        weapons = curWeapons;
        curWepIndex = 0;
    }
    public int _curWepIndex
    {
        get{return curWepIndex;}
    }
    
    public void removeFromInventory(Weapon weapon)
    {
        weapons.Remove(weapon);
    }
    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
    }
    public Weapon getCurrentWeapon()
    {
        return weapons[curWepIndex];
    }
    public Weapon getWeaponAtIndex(int i)
    {
        return weapons[i];
    }
    public void setWeaponIndex(int i)
    {
        this.curWepIndex = i;
    }
    public void ScrollInventory(float scrollWheel)
    {
        if(scrollWheel > 0.99f)
        {
            curWepIndex += 1;
            if(curWepIndex > size)
            {
                curWepIndex = 0;
            }
        }
        else if(scrollWheel < 0.99f)
        {
            curWepIndex -= 1;
            if(curWepIndex < 0)
            {
                curWepIndex = size;
            }
        }
    }
    public int size
    {
        get{return weapons.Count-1;}
    }
    
}
