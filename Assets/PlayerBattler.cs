using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Playables;

public class PlayerBattler : MonoBehaviour
{
    WorldControls _input;

    public Action OnRightAction;
    public Action OnLeftAction;
    public Action OnAttackAction;

    public Transform[] enemies;

    private Vector3 _initialPos;

    public PlayableDirector cine;

    void Start()
    {
        _input = new WorldControls();
        _input.Enable();
        _input.Combat.Right.performed += OnRight;
        _input.Combat.Left.performed += OnLeft;
        _input.Combat.Confirm.performed += OnAttack;
        _initialPos = transform.position;
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
    private void OnAttack(InputAction.CallbackContext ctx)
    {
        if (OnAttackAction != null) OnAttackAction();
        cine.enabled = true;
        Debug.Log("test");
    }

    public void MoveToPos()
    {
        transform.DOMove(enemies[0].transform.position, 0.8f);
    }
    public void ResetPlayerPos()
    {
        transform.position = _initialPos;
        cine.enabled = false;
        cine.time = 0;
    }
}
