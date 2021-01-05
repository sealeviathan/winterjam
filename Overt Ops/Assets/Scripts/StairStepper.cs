using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairStepper : MonoBehaviour
{
    //An abstract class for player stair stepping technology.
    int tracedContacts;
    ContactPoint[] allCPs;
    public float maxStepHeight = 0.4f;        // The maximum a player can set upwards in units when they hit a wall that's potentially a step
    public float stepSearchOvershoot = 0.01f; // How much to overshoot into the direction a potential step in units when testing. High values prevent player from walking up tiny steps but may cause problems.
    private void Start()
    {
        
    }
    public void SetContacts(Collision other)
    {
        tracedContacts = other.contactCount;
        allCPs = new ContactPoint[tracedContacts];
        for(int i = 0; i < tracedContacts;i++)
        {
            allCPs[i] = other.GetContact(i);
        }
    }
    public void ClearContacts()
    {
        allCPs = new ContactPoint[0];
    }
    
    public bool FindGround(out ContactPoint groundCP)
    {
        groundCP = default(ContactPoint);
        bool found = false;
        foreach(ContactPoint contact in allCPs)
        {
            //Pointing with some up direction
            if(contact.normal.y > 0.0001f && (found == false || contact.normal.y > groundCP.normal.y))
            {
                groundCP = contact;
                found = true;
            }
        }
        return found;
    }

    public bool FindStep(out Vector3 stepUpOffset, ContactPoint groundCP, Vector3 curMoveVector)
    {
        stepUpOffset = default(Vector3);
        
        //No chance to step if the player is not moving
        Vector2 movementXZ = new Vector2(curMoveVector.x, curMoveVector.z);
        if(movementXZ == Vector2.zero)
            return false;
        
        foreach(ContactPoint contact in allCPs)
        {
            bool test = ResolveStepUp(out stepUpOffset, contact, groundCP);
            if(test)
                return true;
        }
        return false;
    }
    /// <summary>Takes a contact point that looks as though it's the side face of a step and sees if we can climb it</summary>
    /// <param> stepTestCP ContactPoint to check.</param>
    /// <param> groundCP ContactPoint on the ground.</param>
    /// <param> stepUpOffset The offset from the stepTestCP.point to the stepUpPoint (to add to the player's position so they're now on the step)</param>
    /// <returns> If the passed ContactPoint was a step</returns>
    bool ResolveStepUp(out Vector3 stepUpOffset, ContactPoint stepTestCP, ContactPoint groundCP)
    {
        stepUpOffset = default(Vector3);
        Collider stepCol = stepTestCP.otherCollider; //You'll need the collider of the potential step for this
        //Determine if stepTestCP is a stair...
        //( 1 ) Check if the contact point normal matches that of a stair (y close to 0)
        if(Mathf.Abs(stepTestCP.normal.y) >= 0.01f)
        {
            return false;
        }
        //( 2 ) Make sure the contact point is low enough to be a stair
        if( !(stepTestCP.point.y - groundCP.point.y < maxStepHeight) )
        {
            return false;
        }
        //( 3 ) Check to see if there's actually a place to step in front of us
        //Fires one Raycast
        RaycastHit hitInfo;
        float stepHeight = groundCP.point.y + maxStepHeight + 0.0001f;
        Vector3 stepTestInvDir = new Vector3(-stepTestCP.normal.x, 0, -stepTestCP.normal.z).normalized;
        Vector3 origin = new Vector3(stepTestCP.point.x, stepHeight, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
        Vector3 direction = Vector3.down;
        if( !(stepCol.Raycast(new Ray(origin, direction), out hitInfo, maxStepHeight)) )
        {
            return false;
        }
        //We have enough info to calculate the points
        Vector3 stepUpPoint = new Vector3(stepTestCP.point.x, hitInfo.point.y+0.0001f, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
        Vector3 stepUpPointOffset = stepUpPoint - new Vector3(stepTestCP.point.x, groundCP.point.y, stepTestCP.point.z);
        //We passed all the checks! Calculate and return the point!
        stepUpOffset = stepUpPointOffset;
        return true; //We're going to step up!
    }
}
