using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Camera mainCam;
    private Robot _playerCallback;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerCallback = GetComponent<Robot>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _playerCallback.Thrust(direction);
    }

    public void TurnPlayer(InputAction.CallbackContext context)
    {
        var position = context.ReadValue<Vector2>();
        var mainCamPos = mainCam.ScreenToWorldPoint(position);
        _playerCallback.Turning(mainCamPos);
    }

    public void Fire(InputAction.CallbackContext context)
    {
        _playerCallback.Fire();
    }
}
