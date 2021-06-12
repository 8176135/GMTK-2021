using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public Camera mainCam;
    private Robot _playerCallback;

    private Vector2 _playerAimPos = Vector2.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerCallback = GetComponent<Robot>();
    }

    // Update is called once per frame
    void Update()
    {
        var mainCamPos = mainCam.ScreenToWorldPoint(_playerAimPos);
        _playerCallback.Turning(mainCamPos);
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _playerCallback.Thrust(direction);
    }

    public void TurnPlayer(InputAction.CallbackContext context)
    {
        _playerAimPos = context.ReadValue<Vector2>();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        _playerCallback.Fire();
    }
}
