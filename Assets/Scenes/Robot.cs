using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour
{
    private MainBlock mainBlock;
    
    // Start is called before the first frame update
    void Start()
    {
        mainBlock = this.GetComponent<MainBlock>();
    }
    
    void Thrust(Vector2 thrustValue)
    {
        var thrusters = mainBlock.thrusters;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
