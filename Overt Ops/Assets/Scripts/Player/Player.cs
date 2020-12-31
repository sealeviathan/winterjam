using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKillable, IDamageable
{
    
    public int health {get; set;}
    public int maxHealth {get; set;}
    public float speed = 10.0f;
    public float sprintSpeedMult = 1.5f;
    public float crouchSpeedMult = 0.75f;
    float sprintSpeed;
    float normalSpeed;
    float crouchSpeed;
    public float airSpeed;
    public float jumpSpeed = 5f;
    public float sensitivity = 35f;
    public LayerMask _layerMask;
    Rigidbody rb;
    CapsuleCollider capCol;

    public float footRadius = 1f;
    public float footDistance = 0.5f;
    bool alive = true;
    bool onSlope = false;
    bool grounded = false;
    bool blocked = false;
    bool lastGrounded;

    float wallCheckDist;
    float baseHandHeight;
    public float collisionRadius = 1f;
    public float slopeCheckDist = 0.3f;
    public float maxSlope = 45f;
    public int slopeWalkMod = 10;
    int curFootSlope;
    Vector3 footAngle;

    Vector3 footArea;
    Vector3 bodArea;
    Vector3 headArea;

    Vector3 lastPos;

    Vector3 locomotion
    {
        get{return transform.position-lastPos;}
    }

    Vector3 curAirVector;
    Vector3 lastMoveVector;
    Transform hand;
    

    int jumpsLeft = 0;
    int maxJumpsLeft;
    PlayerCam cam;


    float moveX;
    float moveY;
    public float MoveY{get{return moveY;} set{moveY = value;}}
    public float runBobRate = 5f;
    public float headBobAmount = 1f;

    float jumpCooldown = 0f;
    public float maxJumpCooldown = .25f;

    float crouchScale = 0.75f;
    
    // Start is called before the first frame update
    void Start()
    {
        wallCheckDist = (transform.lossyScale.x + transform.lossyScale.z)/2 * collisionRadius;

        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayerCam>();

        sprintSpeed = sprintSpeedMult * speed;
        normalSpeed = speed;
        crouchSpeed = crouchSpeedMult * speed;
        
        health = 100;
        maxHealth = health;
        maxJumpsLeft = jumpsLeft;
        alive = true;

        hand = gameObject.transform.Find("ItemMount");
        baseHandHeight = hand.transform.localPosition.y;

        footAngle = Vector3.zero;
        capCol = GetComponent<CapsuleCollider>();
        curAirVector = new Vector3();
        lastMoveVector = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {   
        
        if(alive)
        {
            if(jumpCooldown > 0)
            {
                jumpCooldown -= Time.deltaTime;
            }
            //Update is good for getting player input. There are multiple ways to track keypresses and axis, but the input
            //manager is best in this case.
            if(Input.GetButtonDown("Jump"))
            {
                Jump(jumpSpeed);
            }
            CrouchCheck();
            SprintCheck();
            
        }
        
        
    }
    private void FixedUpdate()
    {
        if(alive)
        {
            //NOTE: ?: ternary operator:
            //e.g: var foo = if condition ? valueIfTrue:elseValue;

            moveX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            moveY = Input.GetAxis("Vertical") * Time.deltaTime * speed;

            Vector3 wantedPos = transform.position + transform.forward*moveY + transform.right*moveX;
            Vector3 tryMoveVector = wantedPos - transform.position;

            grounded = Grounded();
            footAngle = GetFloorAngleVector(tryMoveVector);
            onSlope = Helper.GetFloorNormal(transform.position, _layerMask, slopeCheckDist).y != 1 ? true:false;
            //Current problem: you will fetching teleport if you touch a straight down edge at a certain angle and velocity.
            //Possible fix: make onSlope checks more rigid.
            blocked = CheckPlayerBlocked(tryMoveVector, out curFootSlope);
            //While blocked technically works, it could be better.
            //Update to allow wall sliding in the direction of the adjacent angle of trymoveVector and the normal of the
            //object that is in the way. Also, a single ray wont work, use tryMoveVector as a base, and then assign
            //A wider range to capture the odd angles.
            
            //Above moveX and moveY are the -1>1 values from Input.GetAxis() multiplied by player speed and then by the 
            //Frame update stabilizer deltaTime. Always multiply movement by deltaTime.
            if(!blocked)
            {
                if(grounded)
                {
                    if(onSlope)
                    {
                        if(curFootSlope <= maxSlope && curFootSlope >= -maxSlope)
                        {
                            rb.MovePosition(transform.position + footAngle * slopeWalkMod);
                            lastMoveVector = footAngle * slopeWalkMod * airSpeed;
                        }

                    }
                    else
                    {
                        rb.MovePosition(wantedPos);
                        lastMoveVector = tryMoveVector * airSpeed;
                    }
                }
            }
            Debug.Log(onSlope);
            if(!grounded && lastGrounded == true)
            {
                //Forget about velocity, find a way to get a static vector3 of the input when jump was pressed.
                //While not grounded, set the wanted pos to this, and then use whatever air control checks
                //and movements to modify that air vector. Will result in less random death.
                rb.velocity = new Vector3(lastMoveVector.x, rb.velocity.y, lastMoveVector.z);
                
            }
            else if(grounded && lastGrounded == false)
            {
                //Just touched the ground
            }
            lastPos = transform.position;
            lastGrounded = grounded;
            
            //Once everything works as it should, work on slimming this class down, outsource some of the built in methods that
            //Aren't as Player specific.
        }
    }

    public void Damage(int amount)
    {
        //A publicly accessible damage method
        health -= amount;
        if(health < 0)
        {
            Kill();
        }
    }
    public void Kill()
    {
        //Self explanatory
        alive = false;
        rb.constraints = RigidbodyConstraints.None;
    }
    bool Grounded()
    {
        Collider[] hits = Physics.OverlapSphere(footArea, footRadius, _layerMask);
        if(hits.Length > 0)
        {
            grounded = true;
            return true;
        }
        grounded = false;
        return false;
    }

    void Jump(float jumpForce)
    {
        
        //You can change max jumps if you want, it's with regard to air jumps only.
        if(jumpCooldown <= 0)
        {
            if(grounded)
            {
                //Im on the ground
                jumpCooldown = maxJumpCooldown;
                jumpsLeft = maxJumpsLeft;
                onSlope = false;
                grounded = false;
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y + jumpForce, rb.velocity.z);
                //A good way to move rigidbody characters is by directly modifying their velocity. This gives
                //The most responsive web des- er I mean the most responsive character controller.
            }
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
            cam.HeadBob(runBobRate, headBobAmount);
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            speed = normalSpeed;
            cam.headHeight = cam.BaseHeadHeight;
        }
    }
    bool CheckSideArray(Vector3[] origins, Vector3 direction, out bool[] boolHits, out RaycastHit[] rayHits)
    {
        //Check an array of origins when raycasting, e.g; top middle bottom. True if at least half collide.
        int maxSize = origins.Length;

        short count = 0;
        short index = 0;
        boolHits = new bool[maxSize];
        rayHits = new RaycastHit[maxSize];
        foreach(Vector3 origin in origins)
        {
            RaycastHit hit;
            boolHits[index] = false;
            if(Physics.Raycast(origin, direction, out hit, wallCheckDist, _layerMask))
            {
                boolHits[index] = true;
                rayHits[index] = hit;
                count++;
            }
            index++;
        }
        if(grounded)
        {
            return count >= 1;
        }
        else
        {
            return count > 0;
        }
        
    }
    /// <summary> returns true if there is something in the way of said direction of the player, else false </summary>
    bool CheckPlayerBlocked(Vector3 direction, out int angle)
    {
        angle = 0;
        //Choose your origins here
        footArea = new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y/1.5f, transform.position.z);
        bodArea = transform.position;
        headArea = new Vector3(transform.position.x, transform.position.y + transform.lossyScale.y/1.5f, transform.position.z);
        Vector3[] areas = new Vector3[3];
        areas[0] = footArea;
        areas[1] = bodArea;
        areas[2] = headArea;
        //Done with choosing origins, now logic
        bool[] boolHits;
        RaycastHit[] rayHits;
        
        bool inLocomotionPath = CheckSideArray(areas, direction, out boolHits, out rayHits);
        //This whole section basically checks if the player is trying to move into a wall.
        //If they are, stop their movement in that axis.
        if(inLocomotionPath)
        {
            if(grounded)
            {
                //boolHits[0] should always be the feet!!
                if(boolHits[0])
                {
                    //so this is if my feet hit
                    if(Helper.AllFalse(boolHits,1,-1))
                    {
                        //This is if ONLY my feet hit
                        angle = Mathf.CeilToInt(Vector3.Angle(direction, rayHits[0].normal) - 90f);
                        if(angle < maxSlope)
                        {
                            //ultimate step, there is something in my way, im on the ground, its only at my feet, and it isn't
                            //too steep.
                            return false;
                        }
                        return true;
                    }
                    return true;
                }
            }
            return true;
        }
        return false;
    }
    public void SetHandPitch(float angle)
    {
        hand.localEulerAngles = new Vector3(angle, hand.localEulerAngles.y, hand.localEulerAngles.z);
        hand.transform.position = new Vector3(hand.transform.position.x, transform.position.y + baseHandHeight + angle/100, hand.transform.position.z);
    }
    ///<summary>Using Vector3.ProjectOnPlane, finds the perpendicular vector in the direction
    /// of the player to the normal of the floor below the player</summary>
    Vector3 GetFloorAngleVector(Vector3 locomotionDir)
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y/2, transform.position.z);
        Vector3 floorNorm = Helper.GetFloorNormal(origin, _layerMask, slopeCheckDist);
        Vector3 slopeVector = Vector3.ProjectOnPlane(locomotionDir,floorNorm);
        return slopeVector;
        
    }
    
    
    
}
