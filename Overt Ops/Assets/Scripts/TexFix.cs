using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexFix : MonoBehaviour
{
    Renderer rend;
    // Start is called before the first frame update
    //Fixes texture stretching on a per object basis. Just cool really.
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.mainTextureScale = new Vector2(transform.lossyScale.x, transform.lossyScale.z);
    }
}
