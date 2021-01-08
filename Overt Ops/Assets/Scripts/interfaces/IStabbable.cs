using UnityEngine;
public interface IStabbable
{
    //This is an interface that the weapon class uses to interact with the environment.
    //IStabbable allows for things to be stabbed into, like walls, or enemies (or yourself?).
    //Cause damage, calculate angle and and location, stuff like that.

    //NOTE: Look into an inheritable class 'Entity' for players and enemies alike, to save on implemenations.
    void GetStabbed(GameObject other, Vector3 otherDirection);
    
}
