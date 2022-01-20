using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{


    private protected Animator mcAnimator;
    private protected Rigidbody2D mcRigidbody2D;
    private protected BoxCollider2D mcBoxCollider2D;
    private protected SpriteRenderer mcSpriteRenderer;

    [SerializeField] private protected CharacterSObject MainCharacterSObject;

    [SerializeField] private protected LayerMask GameGround;
    [SerializeField] private protected LayerMask GrabPlace;
    [SerializeField] private protected Transform GrabCollider;
    [SerializeField] private protected Transform EdgeGrabCollider;

    enum Movement
    {
        RESTRICTED, ALLOWED
    }

    Movement mcMovement = Movement.ALLOWED;

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

    //SCRIPT ONLY VARIABLES
    private float _moveInput;
    private bool _isFacingRight = true;
    private bool _isWallSliding;
    private bool _isDownSliding;
    private bool _isDashing;


    // END OF SCRIPT ONLY VARIABLES


    [Header("Wall Jumping")]

    //private float _wallJumpTime = 0.2f;  // The delay which allows us to jump.
    private float _wallSlideSpeed = 2f;
    //private float _wallDistance = 0.2f;
    //RaycastHit2D _wallCheckHit;
    //float _jumpTime; // A float to register the full elapsed time with delay.

    void Start()
    {
        //Time.timeScale = 0.5f;
        SetupCom();
    }

    private void SetupCom()
    {
        mcAnimator = gameObject.GetComponent<Animator>();
        mcRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        mcBoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        mcSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();


        Name = MainCharacterSObject.Name;
        Speed = MainCharacterSObject.Speed;
    }

    private void Update()
    {
        SwitchDirection();
        CheckMovement();
    }
    private void FixedUpdate()
    {

    }

    private void SwitchDirection()
    {
        if (_moveInput > 0)
        {
            FlipAnimation(false);
        }
        else if (_moveInput < 0)
        {
            FlipAnimation(true);
        }
    }
    private void FlipAnimation(bool isFlipped)
    {
        if (isFlipped)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            //mcSpriteRenderer.flipX = true;
            //gameObject.transform.eulerAngles = new Vector3(0, 180, 0);
            _isFacingRight = false;
        }
        else
        {
            //gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            //mcSpriteRenderer.flipX = false;
            gameObject.transform.localScale = Vector3.one;
            _isFacingRight = true;
        }
    }
    private void LateUpdate()
    {


    }

    private void CheckMovement()
    {
        if (mcMovement == Movement.RESTRICTED)
        {
            mcAnimator.SetBool("_grabEdge", true);

            mcRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            mcRigidbody2D.velocity = Vector2.zero;

            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow)) && Input.GetAxisRaw("Horizontal") == 0) 
            {
                mcMovement = Movement.ALLOWED;
                if (_isWallSliding)
                {
                    mcAnimator.SetBool("_grabEdge", false);
                    mcAnimator.SetBool("_isSliding", true);
                    mcAnimator.SetBool("_isRunning", false);
                }
                else
                {
                    mcAnimator.SetBool("_grabEdge", false);
                    mcAnimator.SetBool("_isFalling", true);
                    mcAnimator.SetBool("_isRunning", false);
                }
            }
            if ((Input.GetKeyDown(KeyCode.Space) && Input.GetAxisRaw("Horizontal") == 0))
            {
                mcMovement = Movement.ALLOWED;
                mcRigidbody2D.velocity = Vector2.up * 16;
                mcAnimator.SetBool("_grabEdge", false);
                mcAnimator.SetBool("_isSliding", false);
                mcAnimator.SetBool("_isJumping", true);
            }
        }
        else
        {
            mcRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }
        if (mcMovement != Movement.RESTRICTED)
        {
            _moveInput = Input.GetAxis("Horizontal");

            mcRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            mcRigidbody2D.velocity = new Vector2(_moveInput * Speed, mcRigidbody2D.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() || _isWallSliding && Input.GetKeyDown(KeyCode.Space))// || mcMovement == Movement.RESTRICTED) 
        {
            mcRigidbody2D.velocity = Vector2.up * 16;
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt) && IsGrounded() && Mathf.Abs(_moveInput) > 0 && !_isDownSliding && !_isDashing)
        {
            _isDownSliding = true;
            mcAnimator.SetBool("_isRunning", false);
            mcAnimator.SetBool("_isDownSliding", true);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && Mathf.Abs(_moveInput) > 0 && !_isDownSliding && !_isDashing) ////////---------------REMOVED IS GROUNDED HERE
        {
            //
            if(!IsGrounded())
            {
                mcAnimator.SetBool("_isJumping", false);
                mcAnimator.SetBool("_isFalling", false);
                mcAnimator.SetBool("_isDashing", true);
                _isDashing = true;
            }
            _isDashing = true;
            mcAnimator.SetBool("_isRunning", false);
            mcAnimator.SetBool("_isDashing", true);
        }
        if (_isWallSliding)
        {
            mcRigidbody2D.velocity = new Vector2(mcRigidbody2D.velocity.x, Mathf.Clamp(mcRigidbody2D.velocity.y, -_wallSlideSpeed, float.MaxValue));
        }
        if (mcRigidbody2D.velocity.y > 0 && !IsGrounded()) //Check if we are jumping.
        {
            _isDownSliding = false;
            _isDashing = false;
            StopDashImpulse();
            mcAnimator.SetBool("_isJumping", true);
            mcAnimator.SetBool("_isFalling", false);
            mcAnimator.SetBool("_isSliding", false);
            mcAnimator.SetBool("_isDashing", false);
            mcAnimator.SetBool("_isDownSliding", false);

        }
        if (mcRigidbody2D.velocity.y < 0 && !IsGrounded() && !_isDashing) //Check if we are falling.
        {
            _isDownSliding = false;
            _isDashing = false;
            StopDashImpulse();
            mcAnimator.SetBool("_isFalling", true);
            mcAnimator.SetBool("_isDashing", false);
            mcAnimator.SetBool("_isJumping", false);
            mcAnimator.SetBool("_isSliding", false);
            mcAnimator.SetBool("_isDownSliding", false);

        }
        if (IsGrounded()) //Stop fall animation on ground.
        {
            mcAnimator.SetBool("_isFalling", false);
        }
        if (Mathf.Abs(_moveInput) > 0 && IsGrounded())
        {
            mcAnimator.SetBool("_isRunning", true);
        }
        else
        {
            mcAnimator.SetBool("_isRunning", false);
        }
        if (WallHitCheck() && !IsGrounded() && Mathf.Abs(_moveInput) > 0 && mcRigidbody2D.velocity.y < 0)
        {
            _isWallSliding = true;
            mcAnimator.SetBool("_isSliding", true);
            mcAnimator.SetBool("_isFalling", false);
            mcAnimator.SetBool("_isJumping", false);
        }
        else
        {
            _isWallSliding = false;
            mcAnimator.SetBool("_isSliding", false);
            if (!IsGrounded() && mcRigidbody2D.velocity.y < 0)
            {
                mcAnimator.SetBool("_isFalling", true);
            }
        }
        if (GrabPointCheck() && !IsGrounded() && Mathf.Abs(_moveInput) > 0)
        {
            mcMovement = Movement.RESTRICTED;
        }

    }

    public void StopSliding()
    {
        if (Mathf.Abs(_moveInput) > 0)
        {
            _isDownSliding = false;
            mcAnimator.SetBool("_isRunning", true);
            mcAnimator.SetBool("_isDownSliding", false);
        }
        else
        {
            _isDownSliding = false;
            mcAnimator.SetBool("_isDownSliding", false);
        }
    }
    public void StopDashing()
    {
        if (Mathf.Abs(_moveInput) > 0)
        {
            _isDashing = false;
            mcAnimator.SetBool("_isRunning", true);
            mcAnimator.SetBool("_isDashing", false);
        }
        else
        {
            _isDashing = false;
            mcAnimator.SetBool("_isDashing", false);
        }
    }
    public void StopDashInAir()
    {
        if(!IsGrounded())
        {
            _isDashing = false;
        }
    }
    public void DashImpulse()
    {
        Debug.Log("dash power");
        Speed += 4f;
    }
    public void StopDashImpulse()
    {
        Debug.Log("dash power stop");
        Speed = MainCharacterSObject.Speed;
    }

    private bool IsGrounded()
    {
        float boxHieght = 0.1f;
        RaycastHit2D mcRayHit = Physics2D.BoxCast(mcBoxCollider2D.bounds.center, mcBoxCollider2D.bounds.size, 0f, Vector2.down, boxHieght, GameGround);
        return mcRayHit.collider != null;
    }
    private bool WallHitCheck()
    {

        //float boxWidth = 0.1f;
        if (_isFacingRight)
        {
            //RaycastHit2D mcRayWallHit = Physics2D.BoxCast(mcBoxCollider2D.bounds.center, mcBoxCollider2D.bounds.size, 0f, Vector2.right, boxWidth, GameGround);
            RaycastHit2D mcRayWallHit = Physics2D.CircleCast(GrabCollider.transform.position, GrabCollider.GetComponent<CircleCollider2D>().radius, Vector2.right, 0.1f, GameGround);
            return mcRayWallHit.collider != null;
        }
        else
        {
            //RaycastHit2D mcRayWallHit = Physics2D.BoxCast(mcBoxCollider2D.bounds.center, mcBoxCollider2D.bounds.size, 0f, Vector2.left, boxWidth, GameGround);
            RaycastHit2D mcRayWallHit = Physics2D.CircleCast(GrabCollider.transform.position, GrabCollider.GetComponent<CircleCollider2D>().radius, Vector2.right, 0.1f, GameGround);
            return mcRayWallHit.collider != null;
        }
    }
    private bool GrabPointCheck()
    {

        //float boxWidth = 0.1f;
        if (_isFacingRight)
        {
            //RaycastHit2D mcRayWallHit = Physics2D.BoxCast(mcBoxCollider2D.bounds.center, mcBoxCollider2D.bounds.size, 0f, Vector2.right, boxWidth, GameGround);
            RaycastHit2D mcRayWallHit = Physics2D.CircleCast(EdgeGrabCollider.transform.position, EdgeGrabCollider.GetComponent<CircleCollider2D>().radius, Vector2.right, 0.1f, GrabPlace);
            return mcRayWallHit.collider != null;
        }
        else
        {
            //RaycastHit2D mcRayWallHit = Physics2D.BoxCast(mcBoxCollider2D.bounds.center, mcBoxCollider2D.bounds.size, 0f, Vector2.left, boxWidth, GameGround);
            RaycastHit2D mcRayWallHit = Physics2D.CircleCast(EdgeGrabCollider.transform.position, EdgeGrabCollider.GetComponent<CircleCollider2D>().radius, Vector2.right, 0.1f, GrabPlace);
            return mcRayWallHit.collider != null;
        }
    }

}
