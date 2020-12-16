using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Feet))]
public class Player : MonoBehaviour
{
    
    public int health = 10;
    public float speed = 10.0f;
    public float sprintSpeedMult = 1.5f;
    public float crouchSpeedMult = 0.75f;
    float sprintSpeed;
    float normalSpeed;
    float crouchSpeed;
    public float jumpSpeed = 5f;
    public float sensitivity = 35f;
    public LayerMask _layerMask;
    Rigidbody rb;

    bool grounded;

    float wallCheckDist;

    Vector3 footArea;
    Vector3 bodArea;
    Vector3 headArea;

    int jumpsLeft = 2;
    int maxJumpsLeft = 2;
    PlayerCam cam;

    public float collisionRadius = 3f;

    float moveX;
    float moveY;
    public float MoveY{get{return moveY;} set{moveY = value;}}
    public float runBobRate = 5f;

    float crouchScale = 0.75f;
    // Start is called before the first frame update
    void Start()
    {
        wallCheckDist = transform.lossyScale.x + transform.lossyScale.z / collisionRadius;
        rb = GetComponent<Rigidbody>();
        sprintSpeed = sprintSpeedMult * speed;
        normalSpeed = speed;
        crouchSpeed = crouchSpeedMult * speed;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>();
    }

    // Update is called once per frame
    void Update()
    {   
        //Update is good for getting player input. There are multiple ways to track keypresses and axis, but the input
        //manager is best in this case.
        if(Input.GetButtonDown("Jump"))
        {
            Jump(jumpSpeed);
        }
        CrouchCheck();
        SprintCheck();
        
        
    }
    private void FixedUpdate()
    {
        //footArea is used with the below collision checking. determines where the character should raycast from to check
        //if they are hitting a wall.
        footArea = new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y/1.5f, transform.position.z);
        bodArea = transform.position;
        headArea = new Vector3(transform.position.x, transform.position.y + transform.lossyScale.y/1.5f, transform.position.z);
        Vector3[] areas = new Vector3[3];
        areas[0] = footArea;
        areas[1] = bodArea;
        areas[2] = headArea;
        
        moveX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        moveY = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        //Above moveX and moveY are the -1>1 values from Input.GetAxis() multiplied by player speed and then by the 
        //Frame update stabilizer deltaTime. Always multiply movement by deltaTime.
        bool straightOn = CheckSideArray(areas, transform.forward);
        bool rightOn = CheckSideArray(areas, transform.right);
        bool leftOn = CheckSideArray(areas, -transform.right);
        bool backwardsOn = CheckSideArray(areas, -transform.forward);
        
        //This whole section basically checks if the player is trying to move into a wall. If they are, stop their movement in that axis.
        if(straightOn)
        {
            Debug.Log("Straighton");
            if(moveY > 0)
            {
                moveY = 0;
                Debug.Log("Straighton stop");
            }
        }
        if(rightOn)
        {
            if(moveX > 0)
            {
                moveX = 0;
            }
        }
        if(leftOn)
        {
            if(moveX < 0)
            {
                moveX = 0;
            }
        }
        if(backwardsOn)
        {
            if(moveY < 0)
            {
                moveY = 0;
            }
        }
        rb.MovePosition(transform.position + transform.forward*moveY + transform.right*moveX);
    }
    public bool Grounded
    {
        //Simple C# variable public getter setter, adds level of distinction between the class
        //data for protection.
        get{return grounded;}
        set{grounded = value;}
    }

    public void Damage(int amount)
    {
        //A publicly accessible damage method
        health -= amount;
        if(health < 0)
        {
            Die();
        }
    }
    private void Die()
    {
        //Self explanatory
        Destroy(gameObject);
    }

    void Jump(float jumpForce)
    {
        //Uses Feet.cs and a trigger attached to the players feet area to determine if they are on the ground or not.
        //You can change max jumps if you want, it's with regard to air jumps only.
        if(grounded)
        {
            //Im on the ground
            jumpsLeft = maxJumpsLeft;
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForce, rb.velocity.z);
            //A good way to move rigidbody characters is by directly modifying their velocity. This gives
            //The most responsive web des- er I mean the most responsive character controller.
        }
        else
        {
            //Im in the air
            if(jumpsLeft > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForce, rb.velocity.z);
                jumpsLeft--;
            }
        }
        
    }
    //Being made specifically for a capsule player.
    void CrouchCheck()
    {
        if(Input.GetButtonDown("Crouch"))
        {
            float drop = crouchScale / 2;
            transform.position = new Vector3(transform.position.x, transform.position.y - drop, transform.position.z);
            speed = crouchSpeed;
        }
        else if(Input.GetButton("Crouch"))
        {
            //crouch
            speed = crouchSpeed;
            transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
            
        }
        else if(Input.GetButtonUp("Crouch"))
        {
            //uncrouch
            transform.localScale = Vector3.one;
            speed = normalSpeed;
        }
    }
    void SprintCheck()
    {
        if(Input.GetButton("Sprint"))
        {
            speed = sprintSpeed;
            cam.HeadBob(runBobRate, 1.5f);
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            speed = normalSpeed;
            cam.headHeight = cam.BaseHeadHeight;
        }
    }
    bool CheckSideArray(Vector3[] origins, Vector3 direction)
    {
        //Check an array of origins when raycasting, e.g; top middle bottom. True if at least half collide.
        short count = 0;
        foreach(Vector3 origin in origins)
        {
            if(Physics.Raycast(origin, direction, wallCheckDist, _layerMask))
            {
                count++;
            }
        }
        return count > (float)origins.Length/2;
        
    }
    
}
