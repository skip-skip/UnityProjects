using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBoi : MonoBehaviour
{
    [SerializeField] private float baseGravity;
    [SerializeField] private bool debugVectors;

    private Vector2 velocity;
    private Vector2 acceleration;
    private float gravModifier;

    private RaycastHit2D[] castHits = new RaycastHit2D[16];
    private ContactFilter2D filter;
    private const float castExtension = 0.01f;
    private const float minHitDist = 0.001f;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        filter.useTriggers = false;
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        filter.useLayerMask = true;

        velocity = new Vector2();
        acceleration = new Vector2();
        gravModifier = 1;
    }

    
    // Update is called once per frame
    private void Update()
    {
        //Find this frames acceleration and apply it to velocity
        FindIntendedVectors();
        CollisionCheck();
        velocity += acceleration;
        rb.position += velocity;



        if (debugVectors)
        {
            Debug.Log("Velocity: " + velocity.x + ", " + velocity.y);
            Debug.Log("Acceleration: " + acceleration.x + ", " + acceleration.y);
            Debug.DrawRay(rb.position + new Vector2(1, 0), velocity, Color.green);
            Debug.DrawRay(rb.position + new Vector2(1, 0), acceleration, Color.yellow);
        }
    }

    private void FindIntendedVectors()
    {
        CalcAcc();
        velocity += acceleration;
        ExternalVelocity();
    }

    private void CollisionCheck()
    {
        int numHits = rb.Cast(velocity, filter, castHits, (velocity.magnitude + castExtension));

        RaycastHit2D currentHit;
        for(int i = 0; i < numHits; i++)
        {
            currentHit = castHits[i];


            FindContactAcc(currentHit);
            Debug.DrawRay(currentHit.point, currentHit.normal, Color.blue);
        }

    }


    private void CalcAcc()
    {
        acceleration.y = (baseGravity * gravModifier);

        ExternalAcceleration();
    }

    private void FindContactAcc(RaycastHit2D hit)
    {
        Vector2 vectorTowardHit = velocity;
        vectorTowardHit.Normalize();
        vectorTowardHit *= hit.distance;

        acceleration = -(velocity - vectorTowardHit);
    }

    public void SetAcceleration(Vector2 a)
    {
        acceleration = a;
    }
    
    public Vector2 GetAcceleration()
    {
        return acceleration;
    }

    public void SetVelocity(Vector2 v)
    {
        velocity = v;
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }

    public virtual void ExternalAcceleration()
    {

    }
    public virtual void ExternalVelocity()
    {

    }
}
