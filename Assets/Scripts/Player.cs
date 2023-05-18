using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private bool _isInCombatPoint = false;
    public Action OnCombatStart;
    private WorldControls _input;
    void Awake()
    {
        _input = new WorldControls();
        _input.Enable();
        _input.World.Interact.performed += OnInteract;
         
    }

    void Update()
    {
        
    }
    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (_isInCombatPoint)
        {
            if (OnCombatStart != null)
            {
                OnCombatStart();
                GetComponent<PlayerController>().FreezeMovement();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CombatPoint"))
        {
            _isInCombatPoint = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("CombatPoint"))
        {
            _isInCombatPoint = false;
        }
    }
}
