using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon Visual", menuName = "Items/Weapon/Visuals")]
public class WeaponAppearance : ScriptableObject
{
    public Mesh defaultMesh;
    [SerializeField]
    public Vector3 pos;
    [SerializeField]
    public Vector3 rot;
    [SerializeField]
    public Vector3 scale;
    public Material baseMaterial;

    public WeaponAppearance()
    {
        
    }
}
