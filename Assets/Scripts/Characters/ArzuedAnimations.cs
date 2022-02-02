using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArzuedAnimations : MonoBehaviour
{
    private Animator ArzuedAnimator;
    private SpriteRenderer ArzuedSpriteRenderer;
    private Arzued ArzuedBaseScript;
    private ArzuedCollisions ArzuedCollisionsScript;

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
    }

    private void Update()
    {
        ArzuedAnimator.SetBool("_isIdle", ArzuedBaseScript.IsIdle);
        ArzuedAnimator.SetBool("_isMoving", ArzuedBaseScript.IsAbleToMove);
        ArzuedAnimator.SetBool("_isJumping", ArzuedBaseScript.IsJumping);
        ArzuedAnimator.SetBool("_isFalling", ArzuedBaseScript.IsFalling);
        ArzuedAnimator.SetBool("_isGrabbingEdge", ArzuedBaseScript.IsGrabbingEdge);
        ArzuedAnimator.SetBool("_isHanging", ArzuedBaseScript.IsHanging);
        ArzuedAnimator.SetBool("_isWallSliding", ArzuedBaseScript.IsWallSliding);
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
    }
}
