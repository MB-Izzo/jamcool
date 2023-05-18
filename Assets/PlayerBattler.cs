using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBattler : MonoBehaviour
{
    WorldControls _input;

    public Action OnRightAction;
    public Action OnLeftAction;
    void Start()
    {
        _input = new WorldControls();
        _input.Enable();
        _input.Combat.Right.performed += OnRight;
        _input.Combat.Left.performed += OnLeft;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRight(InputAction.CallbackContext ctx)
    {
        if (OnRightAction != null) OnRightAction(); 
    }

    private void OnLeft(InputAction.CallbackContext ctx)
    {
        if (OnLeftAction != null) OnLeftAction(); 
    }
}
