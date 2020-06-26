using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObj : MonoBehaviour
{
    [SerializeField] private int gravMod = 1;
    [SerializeField] private float maxNaturalFallSpeed = -1;


    [SerializeField] private float gravAcceleration = -1;
    private Rigidbody2D rb;
    private Vector2 velocity = new Vector2();

    //Collision Vars
    private RaycastHit2D[] castHits = new RaycastHit2D[16];
    private ContactFilter2D contactFilter;
    private const float CAST_EXTENSION = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    // Update is called once per frame
    protected void Update()
    {
        if(velocity.y > maxNaturalFallSpeed)
            velocity = AddGravity(velocity, gravMod);
        CastAndAdjustVelocity();
        rb.position = rb.position + velocity;
    }

    private Vector2 AddGravity(Vector2 velocity, float mod)
    {
        return velocity + new Vector2(0, gravAcceleration * mod);
    }

    private void CastAndAdjustVelocity()
    {
        CastInDirection(Vector2.down);
        CastInDirection(Vector2.left);
        CastInDirection(Vector2.right);
        void CastInDirection(Vector2 direction)
        {
            int numHits = rb.Cast(direction, contactFilter, castHits, velocity.magnitude + CAST_EXTENSION);
            RaycastHit2D current;
            for (int i = 0; i < numHits; i++)
            {
                current = castHits[i];
                velocity.Normalize();
                velocity *= current.distance;
                Debug.DrawRay(current.point, -velocity, Color.red);
            }
        }
        
    }
}
