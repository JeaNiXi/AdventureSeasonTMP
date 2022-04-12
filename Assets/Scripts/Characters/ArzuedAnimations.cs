using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArzuedAnimations : MonoBehaviour
{
    private Animator ArzuedAnimator;
    private SpriteRenderer ArzuedSpriteRenderer;
    private Arzued ArzuedBaseScript;
    private ArzuedCollisions ArzuedCollisionsScript;
    //private Rigidbody2D ArzuedRigidbody2D;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        ArzuedAnimator = GetComponent<Animator>();
        ArzuedSpriteRenderer = GetComponent<SpriteRenderer>();
        ArzuedBaseScript = GetComponentInParent<Arzued>();
        ArzuedCollisionsScript = GetComponentInParent<ArzuedCollisions>();
        //ArzuedRigidbody2D = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        ArzuedAnimator.SetBool("_isIdle", ArzuedBaseScript.IsIdle);
        ArzuedAnimator.SetBool("_isMoving", ArzuedBaseScript.IsAbleToMove);
        ArzuedAnimator.SetBool("_isJumping", ArzuedBaseScript.IsJumping);
        ArzuedAnimator.SetBool("_isFalling", ArzuedBaseScript.IsFalling);
        ArzuedAnimator.SetBool("_isGrabbingEdge", ArzuedBaseScript.IsGrabbingEdge);
        ArzuedAnimator.SetBool("_isHanging", ArzuedBaseScript.IsHanging);
        ArzuedAnimator.SetBool("_isSliding", ArzuedBaseScript.IsSliding);
        ArzuedAnimator.SetBool("_isDashing", ArzuedBaseScript.IsDashing);
        ArzuedAnimator.SetBool("_isWallSliding", ArzuedBaseScript.IsWallSliding);
        ArzuedAnimator.SetBool("_isAttacking", ArzuedBaseScript.IsAttacking);
        ArzuedAnimator.SetBool("_isDashAttacking", ArzuedBaseScript.IsDashAttacking);
        ArzuedAnimator.SetBool("_isGrounded", ArzuedCollisionsScript.IsGrounded);
    }
    public void Flip(bool flipBool)
    {
        ArzuedSpriteRenderer.flipX = flipBool;
    }
    public void SetHangingStateTrue()
    {
        ArzuedBaseScript.IsHanging = true;
        ArzuedBaseScript.IsGrabbingEdge = false;
        ArzuedBaseScript.IsSliding = false;
    }
    public void AllowMovement()
    {
        ArzuedBaseScript.CanMove = true;
        ArzuedBaseScript.CanAttack = true;
        ArzuedBaseScript.CanDashAttack = false;
    }
    public void StopAttack()
    {
        ArzuedBaseScript.IsAttacking = false;
        ArzuedBaseScript.CanMove = true;
    }
    public void StopDashAttack()
    {
        ArzuedBaseScript.CanDashAttack = false;
        ArzuedBaseScript.IsDashAttacking = false;
        ArzuedBaseScript.CanAttack = true;
        ArzuedBaseScript.CanMove = true;
    }
}
