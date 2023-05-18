using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;

    public Animator animator;
    private CharacterController _controller;
    // Start is called before the first frame update

    private WorldControls _input;

    private Vector3 _move;


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = new WorldControls();
        _input.Enable();
        _input.World.Move.performed += ctx => _move = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _move = _move.normalized;
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forwardRelativeVerticalInput = _move.z * forward;
        Vector3 rightRelativeHorinzontalInput = _move.x * right;
        Vector3 camRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorinzontalInput;
        //        transform.Translate(camRelativeMovement * Time.deltaTime * 5.0f, Space.World);
        if (_controller.enabled)
        {
            _controller.Move(camRelativeMovement * Time.deltaTime * speed);

            if (camRelativeMovement != Vector3.zero)
            {
                Quaternion toRot = Quaternion.LookRotation(camRelativeMovement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, 1000.0f * Time.deltaTime);
                animator.SetBool("isRunning", true);
            }
            else 
            {
                animator.SetBool("isRunning", false);
            }

        }
    }

    public void FreezeMovement()
    {
        _input.World.Move.Disable();
        animator.SetBool("isRunning", false);
        _controller.enabled = false;
    }

    public void UnFreezeMovement()
    {
        _input.World.Move.Enable();
        _controller.enabled = true;
    }
}
