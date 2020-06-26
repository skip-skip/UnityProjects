using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerBoi : PhysicsBoi
{
    [SerializeField] private float moveAcceleration;

    private float leftRight;
    private Vector2 inputAcceleration;

    private void ReadInput()
    {
        leftRight = Input.GetAxisRaw("Horizontal");
    }

    public override void ExternalAcceleration()
    {
        inputAcceleration = GetAcceleration();

        inputAcceleration += new Vector2(leftRight * moveAcceleration, 0);
        Debug.Log(inputAcceleration);
        SetAcceleration(inputAcceleration);
    }

    public override void ExternalVelocity()
    {
        base.ExternalVelocity();
    }


}
