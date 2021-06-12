using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Robot moveCallback;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        moveCallback.Thrust(direction);
    }

    public void TurnPlayer(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        // moveCallback.Turning(direction);
    }
}
