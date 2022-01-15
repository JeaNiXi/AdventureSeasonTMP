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
    private bool _isFacingRight;

    [Header("Wall Jumping")]

    [SerializeField] private float _wallJumpTime = 0.2f;  // The delay which allows us to jump.
    [SerializeField] private float _wallSlideSpeed = 2f;
    [SerializeField] private float _wallDistance = 0.55f;
    [SerializeField] private bool _isWallSliding;
    RaycastHit2D _wallCheckHit;
    float _jumpTime; // A float to register the full elapsed time with delay.

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
            mcRigidbody2D.velocity = new Vector2(_moveInput * Speed, mcRigidbody2D.velocity.y);


        //}

        //Wall Jump
        if(_isFacingRight)
        {
            _wallCheckHit = Physics2D.Raycast(gameObject.transform.position, new Vector2(_wallDistance, 0), _wallDistance, GameGround);
        }
        else
        {
            _wallCheckHit = Physics2D.Raycast(gameObject.transform.position, new Vector2(-_wallDistance, 0), _wallDistance, GameGround);
        }
        if (_wallCheckHit && !IsGrounded() && _moveInput != 0)
        {
            _isWallSliding = true;
            mcAnimator.SetBool("_isSliding", true);
            _jumpTime = Time.time + _wallJumpTime;
        }
        else if (_jumpTime < Time.time)
        {
            _isWallSliding = false;
            mcAnimator.SetBool("_isSliding", false);
        }
        if (_isWallSliding)
        {
            mcRigidbody2D.velocity = new Vector2(mcRigidbody2D.velocity.x, Mathf.Clamp(mcRigidbody2D.velocity.y, _wallSlideSpeed, float.MaxValue));
        }

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
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())// || _isWallSliding && Input.GetKeyDown(KeyCode.Space)) 
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
            _isFacingRight = false;
        }
        else
        {
            gameObject.transform.localScale = Vector3.one;
            _isFacingRight = true;
        }
    }
}
