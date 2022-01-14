using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{


    private protected Animator mcAnimator;
    private protected Rigidbody2D mcRigidbody2D;
    private protected BoxCollider2D mcBoxCollider2D;

    [SerializeField] private protected CharacterSObject MainCharacterSObject;

    [SerializeField] private protected LayerMask GameGround;


    private string _name;
    private float _speed;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }
    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }


    private float _moveInput;

    void Start()
    {
        SetupCom();
    }

    private void SetupCom()
    {
        mcAnimator = gameObject.GetComponent<Animator>();
        mcRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        mcBoxCollider2D = gameObject.GetComponent<BoxCollider2D>();


        Name = MainCharacterSObject.Name;
        Speed = MainCharacterSObject.Speed;
    }

    private void Update()
    {
        CheckMovement();
    }
    private void FixedUpdate()
    {
        //Checking directional movements.
        //if (true) 
        //{
            mcRigidbody2D.velocity = new Vector2(_moveInput * Speed, mcRigidbody2D.velocity.y);
        //}
    }
    private void LateUpdate()
    {
        //Switching direction of player.
        if (_moveInput > 0)
        {
            FlipAnimation(false);
        }
        else if (_moveInput < 0)
        {
            FlipAnimation(true);
        }
    }

    private void CheckMovement()
    {
        _moveInput = Input.GetAxis("Horizontal");
        mcRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        //mcRigidbody2D.velocity = new Vector2(_moveInput * Speed, mcRigidbody2D.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            mcRigidbody2D.velocity = Vector2.up * 10;
        }
        if (Mathf.Abs(_moveInput) > 0 && IsGrounded())
        {
            mcAnimator.SetBool("_isRunning", true);
        }
        else
        {
            mcAnimator.SetBool("_isRunning", false);
        }
        if (mcRigidbody2D.velocity.y == 0 || IsGrounded())
        {
            mcAnimator.SetBool("_isFalling", false);
            mcAnimator.SetBool("_isJumping", false);
        }
        if (mcRigidbody2D.velocity.y > 0 && !IsGrounded())
        {
            mcAnimator.SetBool("_isJumping", true);
        }
        if (mcRigidbody2D.velocity.y < 0 && !IsGrounded())
        {
            mcAnimator.SetBool("_isJumping", false);
            mcAnimator.SetBool("_isFalling", true);
        }
    }
    private bool IsGrounded()
    {
        float boxHieght = 0.1f;
        RaycastHit2D mcRayHit = Physics2D.BoxCast(mcBoxCollider2D.bounds.center, mcBoxCollider2D.bounds.size, 0f, Vector2.down, boxHieght, GameGround);
        Debug.Log(mcRayHit.collider);
        return mcRayHit.collider != null;
    }

    private void FlipAnimation(bool isFlipped)
    {
        if (isFlipped)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = Vector3.one;
        }
    }
}
