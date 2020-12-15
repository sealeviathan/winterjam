using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    //A built in unity world method, calls when this object's box collider is set to trigger,
    //and another collider enters that trigger.
    private void OnTriggerEnter(Collider other) {
        if(other.tag != "Player")
        {
            _player.Grounded = true;
        }
    }
    //See trigger enter, but when it exits.
    private void OnTriggerExit(Collider other) {
        if(other.tag != "Player")
        {
            _player.Grounded = false;
        }
    }
}
