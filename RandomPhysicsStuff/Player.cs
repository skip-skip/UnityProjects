using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : PhysicsObj
{

    private PlayerInput controls;
    private Vector2 moveDirection;


    private void Awake()
    {
        controls = new PlayerInput();
        

    }

    private void HandleMove(InputAction.CallbackContext cntx)
    {
        moveDirection = cntx.ReadValue<Vector2>();
        Debug.Log(moveDirection);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

   

    private void OnEnable()
    {
        controls.Player.Move.performed += HandleMove;
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Move.performed -= HandleMove;
        controls.Disable();
    }
}
