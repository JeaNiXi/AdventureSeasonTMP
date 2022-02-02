using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArzuedCollisions : MonoBehaviour
{
    [Header("Layers")]

    public LayerMask GroundLayer;
    public LayerMask GrabPlace;

    [Space]
    public bool IsGrounded;
    public bool IsOnWall;
    public bool IsOnRightWall;
    public bool IsOnLeftWall;
    public bool IsGrabbingRight;
    public bool IsGrabbingLeft;
    public bool IsHittingHead;

    [Space]
    [Header("Collisions")]

    private float collisionRadius = 0.3f;
    private float sideRadius = 0.08f;
    private float grabRagius = 0.10f;
    private Vector3 boxSize = new Vector3(0.5f,0.5f,0.5f);

    public Vector2 bottomOffset, rightOffset, leftOffset, upperOffset;
    
    private Color debugCollisionColor = Color.red;

    private void Update()
    {
        IsGrounded = Physics2D.OverlapCircle((Vector2)gameObject.transform.position + bottomOffset, collisionRadius, GroundLayer);
        IsOnRightWall = Physics2D.OverlapCircle((Vector2)gameObject.transform.position + rightOffset, sideRadius, GroundLayer);
        IsOnLeftWall =  Physics2D.OverlapCircle((Vector2)gameObject.transform.position + leftOffset, sideRadius, GroundLayer);
        IsHittingHead = Physics2D.OverlapBox((Vector2)gameObject.transform.position + upperOffset, boxSize, 0f, GroundLayer);

        IsGrabbingRight = Physics2D.OverlapCircle((Vector2)gameObject.transform.position + rightOffset, grabRagius, GrabPlace);
        IsGrabbingLeft = Physics2D.OverlapCircle((Vector2)gameObject.transform.position + leftOffset, grabRagius, GrabPlace);

        if (IsOnLeftWall || IsOnRightWall)
        { 
            IsOnWall = true;
        }
        else
        {
            IsOnWall = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, sideRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, sideRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, grabRagius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, grabRagius);
        Gizmos.DrawWireCube((Vector2)transform.position + upperOffset, boxSize);

    }
}
