using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    public Camera mainCam;
    public Robot robot;

    public UIDocument docs;
    public Label scoreLabel;
    
    private Vector2 _playerAimPos = Vector2.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        robot = GetComponent<Robot>();
        docs = FindObjectOfType<UIDocument>();
        scoreLabel = docs.rootVisualElement.Q<Label>("ScoreVal");
    }

    // Update is called once per frame
    void Update()
    {
        var mainCamPos = mainCam.ScreenToWorldPoint(_playerAimPos);
        robot.Turning(mainCamPos);
        robot.SetAimTarget(mainCamPos);
        scoreLabel.text = robot.mainBlock.BlockCount.ToString();
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        robot.Thrust(direction);
    }

    public void TurnPlayer(InputAction.CallbackContext context)
    {
        _playerAimPos = context.ReadValue<Vector2>();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        var pressed = context.ReadValueAsButton();
        robot.Fire(pressed);
    }
}
