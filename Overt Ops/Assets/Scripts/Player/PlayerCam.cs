using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public GameObject player;
    Vector3 thirdPersonTarget;
    Vector3 firstPersonHead;
    public float headHeight;
    float baseHeadHeight;
    public float BaseHeadHeight{get{return baseHeadHeight;}}
    public bool camShift = false;

    public float mouseSens = 100f;
    public float camSmoothing = 10f;
    float rotX = 0;

    Player _Player;
    void Start()
    {
        //Instead of choosing target in the inspector, find whatever object is tagged 'Player' and set it
        //to our target gameobject.
        player = GameObject.FindGameObjectWithTag("Player");
        rotX = 0;
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        baseHeadHeight = headHeight;
        _Player = player.GetComponent<Player>();
    }


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 100f))
        {
        }
    }
    //This one is upsetting, but basically converts a regular +- angle scale to the weird quaternion 360 degree euler degree thing.
    //It just works.
    //The value should be on a -x +x scale, where min = -x and max = +x
    float EulerToQuaternionRotBound(float value, float min, float max)
    {
        if(value < min)
        {
            return min + 360;
        }
        else if(value > max)
        {
            return max;
        }
        else
        {
            if(value < 0)
            {
                return value + 360;
            }
            return value;
        }
    }
    
    private void FixedUpdate()
    {
         //Fixed update is very similar to update, except that you should use it only and always when working with
         //anything physics related. Rigidbodies, raycasts, etc...
         //First I set a new position in space called firstPersonHead, which is the player position plus some
         //dynamic head height.
        firstPersonHead = new Vector3(player.transform.position.x, player.transform.position.y + headHeight, player.transform.position.z);
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        //these variables hold the mouse input, ranging from -1 > 1, multiplies it by sensitivity, and then by 
        //Time.deltaTime so it is constant regardless of framerate
        rotX += mouseY;
        rotX = Mathf.Clamp(rotX, -90f, 90f);
        float camRotX = EulerToQuaternionRotBound(rotX, -90f, 90f);
        float camRotY = transform.localEulerAngles.y + mouseX;
        //These values contain the literal rotation value of the camera to be applied.
        Vector3 mouseMovement = new Vector3(camRotX, camRotY, transform.localEulerAngles.z);
        transform.position = firstPersonHead;
        transform.localEulerAngles = mouseMovement;
        player.transform.localEulerAngles = new Vector3(player.transform.localEulerAngles.x, transform.localEulerAngles.y, player.transform.localEulerAngles.z);
        _Player.SetHandPitch(-rotX);
        
        //When scripting, the above 'transform.xyz' is referencing the TRANSFORM of the GAMEOBJECT this script is attached to;
        //no need for getComponent in this case. Except for rotating the player; this requires our aforementioned gameobject assignation
        //from the start method.
        //Sets this GAMEOBJECT's TRANSFORM component's position to the previously declared firstPersonHead
        //Sets this GAMEOBJECT's TRANSFORM component's rotation in local euler angles (AKA 0>360 deg. on 3 axis)
        //Sets the player GAMEOBJECT's TRANSFORM component's rotation in local euler angles (AKA 0>360 deg. on 3 axis)
        
    }

    public void HeadBob(float rate, float bobAmount)
    {
        headHeight = bobAmount * Mathf.Sin(Time.time * rate) * _Player.MoveY + baseHeadHeight;
    }
    

    
}
