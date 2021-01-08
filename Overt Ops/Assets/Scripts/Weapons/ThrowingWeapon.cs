using UnityEngine;

public class ThrowingWeapon
{
    //Specifically, thrown members
    GameObject thrownObject;
    float thrownVelocityMult;
    float thrownArcMult;
    float effectiveDistance;

    public bool empty;
    public ThrowingWeapon()
    {
        empty = true;
    }
    public ThrowingWeapon(GameObject thrownObject, float thrownVelocityMult, float thrownArcMult = 1.5f, float effectiveDistance = 10f)
    {
        empty = false;
        this.thrownObject = thrownObject;
        this.thrownVelocityMult = thrownVelocityMult;
        this.thrownArcMult = thrownArcMult;
        this.effectiveDistance = effectiveDistance;
    }
    public void UpdateThrowingTimers(float timeScale)
    {

    }
    public void Throw(Vector3 origin, Vector3 direction, out bool isAvaliable)
    {
        isAvaliable = true;
        //The idea is to add a vector3.up multiplied by arcmult to the direction times the velocity
        //mult, this way, instead of throwing in a boring straight line, the item is tossed in
        //more of an arc fashion. Or, similarly, if you want no arc, multiply the arc by 0,
        //and it will throw straight.
        if(thrownObject != null)
        {
            isAvaliable = false;
            Vector3 throwVector = direction * thrownVelocityMult + Vector3.up * thrownArcMult;
        }
        else
        {
            Debug.LogError("Trying to throw a weapon with no thrown object item attached!");
        }
  
    }
}
