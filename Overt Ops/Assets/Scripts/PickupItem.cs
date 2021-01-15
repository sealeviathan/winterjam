using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class PickupItem : MonoBehaviour
{
    public Mesh visual;
    public float rotSpeed;
    public float bobSpeed;
    public enum Types{Weapon,Recharge,Objective}
    public Types thisType;
    public Weapon possibleWeapon;
    float initialHeight;
    // Start is called before the first frame update
    //Need to autofetch the texture for the mesh renderer
    void Start()
    {
        BoxCollider thisCollider = GetComponent<BoxCollider>();
        thisCollider.isTrigger = true;
        if(possibleWeapon != null)
        {
            thisType = Types.Weapon;
        }
        //...
        MeshFilter _meshFilter = GetComponent<MeshFilter>();
        _meshFilter.mesh = visual;
        initialHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float rot = rotSpeed*Time.deltaTime;
        float pos = Mathf.Cos(Time.time * bobSpeed);
        Vector3 newRot = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + rot, transform.localEulerAngles.z);
        Vector3 newPos = new Vector3(transform.position.x, pos + initialHeight, transform.position.z);
        transform.position = newPos;
        transform.localEulerAngles = newRot;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            if(thisType == Types.Weapon)
            {
                Player _player = other.GetComponent<Player>();
                _player.GiveWeapon(possibleWeapon);
            }
        }
    }
}
