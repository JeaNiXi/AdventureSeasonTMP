using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arzued : MonoBehaviour
{
    // Adding basic connections.
    Rigidbody2D ArzuedRigidbody2D;
    BoxCollider2D ArzuedBoxCollider2D;
    ArzuedAnimations ArzuedAnimationScript;
    ArzuedCollisions ArzuedCollisionsScript;


    enum Movement
    {
        RESTRICTED,
        ALLOWED
    }
    enum Direction
    {
        RIGHT,
        LEFT
    }

    [SerializeField] Movement arzuedMovement = Movement.ALLOWED;
    [SerializeField] Direction arzuedDirection = Direction.RIGHT;

    // Character vars.

    //private string _name = "Arzued";
    private float _moveSpeed = 10.0f;
    private float _jumpForce = 10.0f;
    private float _wallSlideSpeed = 2.0f;
    private float _wallJumpForce = 10.0f;
    private float _slideTime = 0.6f;
    private float _dashAnimationTime = 0.1f;
    private float _dashStopTime = 0.3f;
    private float _dashImpulse = 10.0f;

    // Naming code vars.
    private float _xInput;
    private float _yInput;
    private Vector2 _inputVector;
    private Vector2 _wallJumpVector;

    // Booleans
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _isIdle;
    private bool _isMoving;
    private bool _isJumping;
    private bool _isFalling;
    [SerializeField] private bool _isGrabbingEdge;
    [SerializeField] private bool _isHanging;
    private bool _hasDashJumped;
    private bool _isSliding;
    private bool _isDashing;
    private bool _isAttacking;
    private bool _canAttack;
    private bool _isDashAttacking;
    [SerializeField] private bool _canDashAttack;

    public bool _isWallSliding;
    public bool _isWallJumping;

    // Properties
    public bool CanMove
    {
        get
        {
            return _canMove;
        }
        set
        {
            _canMove = value;
        }
    }
    public bool IsIdle
    {
        get
        {
            return _isIdle;
        }
        set
        {
            _isIdle = value;
        }
    }
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
        }
    }
    public bool IsAbleToMove
    {
        get
        {
            return _isMoving;
        }
    }
    public bool IsJumping
    {
        get
        {
            return _isJumping;
        }
        set
        {
            _isJumping = value;
        }
    }
    public bool IsFalling
    {
        get
        {
            return _isFalling;
        }
        set
        {
            _isFalling = value;
        }
    }
    public bool IsWallSliding
    {
        get
        {
            return _isWallSliding;
        }
        set
        {
            _isWallSliding = value;
        }
    }
    public bool IsGrabbingEdge
    {
        get
        {
            return _isGrabbingEdge;
        }
        set
        {
            _isGrabbingEdge = value;
        }
    }
    public bool IsHanging
    {
        get
        {
            return _isHanging;
        }
        set
        {
            _isHanging = value;
        }
    }
    public bool IsSliding
    {
        get
        {
            return _isSliding;
        }
        set
        {
            _isSliding = value;
        }
    }
    public bool IsDashing
    {
        get
        {
            return _isDashing;
        }
        set
        {
            _isDashing = value;
        }
    }
    public bool IsAttacking
    {
        get
        {
            return _isAttacking;
        }
        set
        {
            _isAttacking = value;
        }
    }
    public bool CanAttack
    {
        get
        {
            return _canAttack;
        }
        set
        {
            _canAttack = value;
        }
    }
    public bool IsDashAttacking
    {
        get
        {
            return _isDashAttacking;
        }
        set
        {
            _isDashAttacking = value;
        }
    }
    public bool CanDashAttack
    {
        get
        {
            return _canDashAttack;
        }
        set
        {
            _canDashAttack = value;
        }
    }

    private void Start()
    {
        Initialize();
        //Time.timeScale = 0.2f;
    }

    private void Initialize()
    {
        ArzuedRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        ArzuedBoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        ArzuedAnimationScript = gameObject.GetComponentInChildren<ArzuedAnimations>();
        ArzuedCollisionsScript = gameObject.GetComponent<ArzuedCollisions>();

        StartGame();
    }

    private void StartGame()
    {
        _canMove = true;
        _canAttack = true;
    }

    private void Update()
    {
        //Debug.Log(ArzuedRigidbody2D.velocity.y);
        //ArzuedRigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (arzuedMovement == Movement.RESTRICTED)
        {
            if (Input.GetButtonDown("Jump"))
            {
                IsHanging = false;
                IsGrabbingEdge = false;
                IsJumping = true;
                Debug.Log("Do jump grab");
                arzuedMovement = Movement.ALLOWED;
                Jump(Vector2.up, _jumpForce);
            }
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) && _xInput == 0))
            {
                arzuedMovement = Movement.ALLOWED;
            }
            if (IsFalling)
            {
                IsHanging = false;
                IsGrabbingEdge = false;
                arzuedMovement = Movement.ALLOWED;
            }
        }
        else
        {
            if (!IsAttacking)
            {
                ArzuedRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                IsHanging = false;
                IsGrabbingEdge = false;
            }
        }


        // Basic horizontal movement.
        if (arzuedMovement != Movement.RESTRICTED && CanMove)
        {
            _xInput = Input.GetAxis("Horizontal");
            _yInput = Input.GetAxis("Vertical");
            _inputVector = new Vector2(_xInput, _yInput);

        }
        if (!_isWallSliding || (!ArzuedCollisionsScript.IsGrounded && _inputVector.y != 0))
        {
            Move(_inputVector);
        }


        // Check for jumping.
        if (Input.GetButtonDown("Jump") && ArzuedCollisionsScript.IsGrounded && arzuedMovement != Movement.RESTRICTED && !IsSliding && !IsDashing && CanMove && !IsAttacking)
        {
            Jump(Vector2.up, _jumpForce);
        }

        // Check for jumping from wall.
        if (Input.GetButtonDown("Jump") && _isWallSliding && arzuedMovement != Movement.RESTRICTED)
        {
            WallJump();
        }

        // Check for dashing while jumping.

        if (Input.GetButtonDown("Fire2") && !ArzuedCollisionsScript.IsGrounded && !_hasDashJumped && !IsWallSliding && !IsHanging)
        {
            DashJump();
        }

        // Check for sliding.
        if (Input.GetKeyDown(KeyCode.LeftControl) && ArzuedCollisionsScript.IsGrounded && !IsHanging && Mathf.Abs(ArzuedRigidbody2D.velocity.y) < 1f && !IsIdle && !IsWallSliding && !IsDashing && !IsSliding && CanMove)
        {
            Slide();
        }
        // Check for sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift) && ArzuedCollisionsScript.IsGrounded && !IsHanging && Mathf.Abs(ArzuedRigidbody2D.velocity.y) < 1f && !IsIdle && !IsWallSliding && !IsDashing && !IsSliding && CanMove)
        {
            Dash();
        }
        // Check for Jumping/Falling
        if (ArzuedRigidbody2D.velocity.y > 0 && !ArzuedCollisionsScript.IsGrounded)
        {
            _isJumping = true;
            _isFalling = false;
            _isIdle = false;
            IsSliding = false;
            IsHanging = false; // added
            CanMove = true;
        }
        if (ArzuedRigidbody2D.velocity.y < 0 && !ArzuedCollisionsScript.IsGrounded)
        {
            _isFalling = true;
            _isJumping = false;
            _isMoving = false;
            _isIdle = false;
            _isSliding = false;
            CanMove = true;
        }
        //// Check for sliding or dashing up or down.
        if (Mathf.Abs(ArzuedRigidbody2D.velocity.y) > 1f && ArzuedCollisionsScript.IsGrounded && (IsSliding || IsDashing))
        {
            Debug.Log("sliding up");
            if (IsSliding)
            {
                IsSliding = false;
            }
            else
            {
                IsDashing = false;
            }
            ArzuedRigidbody2D.velocity = Vector2.zero;
            //    CanMove = false;
        }
        // Check if character is grounded.
        if (ArzuedCollisionsScript.IsGrounded)
        {
            _isFalling = false;
            _isJumping = false;
            _isWallSliding = false;
            ResetDash();
        }
        else
        {
            _isIdle = false;
        }

        // Check if character is idle.
        if (_inputVector == Vector2.zero && !_isFalling && !IsAttacking)
        {
            _isIdle = true;
        }

        // Check for wall sliding.
        if ((ArzuedCollisionsScript.IsOnLeftWall && _inputVector.x < 0 && !ArzuedCollisionsScript.IsGrounded && !ArzuedCollisionsScript.IsHittingHead) || (ArzuedCollisionsScript.IsOnRightWall && _inputVector.x > 0 && !ArzuedCollisionsScript.IsGrounded && !ArzuedCollisionsScript.IsHittingHead))
        {
            _isWallSliding = true;
            _isFalling = false;
            WallSlide();
        }
        else if (!ArzuedCollisionsScript.IsGrounded)
        {
            _isWallSliding = false;
            _isFalling = true;
        }
        else
        {
            _isWallSliding = false;
        }
        if (((ArzuedCollisionsScript.IsGrabbingRight && arzuedDirection == Direction.RIGHT) || (ArzuedCollisionsScript.IsGrabbingLeft && arzuedDirection == Direction.LEFT)) && !IsJumping && Mathf.Abs(_inputVector.x) > 0) 
        {
            GrabEdge();
        }
        
        // ATTACKS AND ETC
        // Checking for attack
        if(Input.GetButtonDown("Fire1") && ArzuedCollisionsScript.IsGrounded && !IsAttacking && !IsSliding && !IsHanging && !IsGrabbingEdge && !IsFalling && !IsJumping && _canAttack && !IsDashAttacking)
        {
            if (!IsDashing)
            {
                ArzuedRigidbody2D.velocity = Vector2.zero;
                ArzuedRigidbody2D.bodyType = RigidbodyType2D.Static;
                IsIdle = false;
                CanMove = false;
                IsMoving = false;
                IsAttacking = true;
            }
        }
        if(Input.GetButtonDown("Fire1") && CanDashAttack)
        {
            IsDashAttacking = true;
        }
    }

    // Basic Movement Functions.

    private void Move(Vector2 direction)
    {
        if(!_canMove)
        {
            return;
        }
        if (_inputVector.x > 0)
        {
            arzuedDirection = Direction.RIGHT;
            ArzuedAnimationScript.Flip(false);
            _isMoving = true;
            _isIdle = false;
        }
        else if (_inputVector.x < 0)
        {
            arzuedDirection = Direction.LEFT;
            ArzuedAnimationScript.Flip(true);
            _isMoving = true;
            _isIdle = false;
        }
        else
        {
            _isMoving = false;
        }
        ArzuedRigidbody2D.velocity = new Vector2(_inputVector.x * _moveSpeed, ArzuedRigidbody2D.velocity.y);
    }
    private void Jump(Vector2 direction, float jumpForce)
    {
        ArzuedRigidbody2D.velocity = direction * jumpForce;
    }
    private void WallSlide()
    {
        ResetDash();

        if (_inputVector.x != 0 && !_isWallJumping && ArzuedRigidbody2D.velocity.y <= 0) //!Input.GetButton("Jump"))
        {
            ArzuedRigidbody2D.velocity = Vector2.up * -_wallSlideSpeed;
        }
    }
    private void WallJump()
    {
        ResetDash();

        _isWallJumping = true;
        if (ArzuedCollisionsScript.IsOnLeftWall)
        {
            _wallJumpVector = Vector2.right;
            StartCoroutine(DisableMovement(.3f));
            Jump(_wallJumpVector/2 + Vector2.up, _wallJumpForce);
            //_isWallJumping = false;
        }
        if (ArzuedCollisionsScript.IsOnRightWall)
        {
            _wallJumpVector = Vector2.left;
            StartCoroutine(DisableMovement(.4f));
            Jump(_wallJumpVector/2 + Vector2.up, _wallJumpForce);
            //_isWallJumping = false;
        }
    }
    //
    // Wall Jumping from one wall to another bug can occur. If the wall slide is not enabled in time.
    //
    // Enumarators
    private void GrabEdge()
    {
        ResetDash();

        _isFalling = false;
        _isWallSliding = false;
        _isMoving = false;
        _isIdle = false;
        if (!IsHanging)
        {
            _isGrabbingEdge = true;
        }

        arzuedMovement = Movement.RESTRICTED;
        //ArzuedRigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        ArzuedRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        ArzuedRigidbody2D.velocity = Vector2.zero;
    }
    private void DashJump()
    {
        _hasDashJumped = true;
        ArzuedRigidbody2D.velocity = Vector2.zero;
        ArzuedRigidbody2D.velocity += Vector2.up * 12f;
    }
    private void ResetDash()
    {
        _hasDashJumped = false;
    }
    private void Slide()
    {
        CanMove = false;
        IsSliding = true;
        StartCoroutine(StopSliding(_slideTime));
    }
    private void Dash()
    {
        CanMove = false;
        CanAttack = false;
        IsDashing = true;
        CanDashAttack = true;
        StartCoroutine(DashController());
        //StartCoroutine(StopDashing(_dashTime));
    }
    private IEnumerator DashController()
    {
        while (!IsDashAttacking)
        {
            if (arzuedDirection == Direction.RIGHT)
            {
                ArzuedRigidbody2D.AddForce(Vector2.right * _dashImpulse, ForceMode2D.Impulse);
            }
            else
            {
                ArzuedRigidbody2D.AddForce(Vector2.left * _dashImpulse, ForceMode2D.Impulse);
            }
            //float temp = _moveSpeed;
            //_moveSpeed = _dashSpeed;
            yield return new WaitForSeconds(_dashAnimationTime);
            IsDashing = false;
            yield return new WaitForSeconds(_dashStopTime - _dashAnimationTime);
            ArzuedRigidbody2D.velocity = Vector2.zero;
            //_moveSpeed = temp;
            yield break;
        }
        yield break;
    }
    private IEnumerator StopSliding(float slideTime)
    {
        yield return new WaitForSeconds(slideTime);
        IsSliding = false;
        yield break;    
    }
    private IEnumerator DisableMovement(float time)
    {
        _canMove = false;
        if(ArzuedCollisionsScript.IsOnLeftWall)
        {
            ArzuedAnimationScript.Flip(false);
        }
        else
        {
            ArzuedAnimationScript.Flip(true);
        }
        yield return new WaitForSeconds(time);
        _isWallJumping = false;
        _canMove = true;
        yield break;
    }
}












//private ArzuedCollisions aCollisions;
//private Rigidbody2D aRigidBody;




//#region Variables
//[Space]
//[Header("Basic")]

//public string baseName = "Arzued";
//public float speed=8f;
//public float jumpForce=6f;
//public float slideSpeed = 3f;
//public float climbSpeed = 4f;
//public float wallJumpLerp = 10f;

//[Space]
//[Header("Booleans")]

//public bool IsGrounded;
//public bool IsOnWall;
//public bool IsGrabbingWall;
//public bool IsWallJumping;
//public bool IsAbleToMove = true;



//#endregion

//private void Setup()
//{
//    aCollisions = gameObject.GetComponent<ArzuedCollisions>();
//    aRigidBody = gameObject.GetComponent<Rigidbody2D>();

//}

//private void Start()
//{
//    Setup();
//}

//private void Update()
//{
//    CheckInput();



//}

//private void CheckInput()
//{
//    float xInput, yInput;
//    xInput = Input.GetAxisRaw("Horizontal");
//    yInput = Input.GetAxisRaw("Vertical");

//    Vector2 inputVector = new Vector2(xInput, yInput);
//    Walk(inputVector);


//    if(Input.GetButtonDown("Jump"))
//    {
//        if(aCollisions.IsGrounded)
//        {
//            Jump(Vector2.up);
//        }
//        else //Can add if(aCollisions.Os on wall && !not Grounded)
//        {
//            WallJump();
//        }
//    }
//    if(Input.GetKeyDown(KeyCode.LeftAlt))
//    {
//        Dash(xInput, yInput);
//    }

//    if (aCollisions.IsOnWall && !aCollisions.IsGrounded)
//    {
//        WallSlide();
//    }

//    IsGrabbingWall = aCollisions.IsOnWall && Input.GetKey(KeyCode.LeftShift);

//    if(IsGrabbingWall)
//    {
//        ClimbWall();
//    }
//}
//#region BasicMovement
//private void Walk(Vector2 xyInput)
//{
//    if (!IsWallJumping)
//    {
//        aRigidBody.velocity = new Vector2((xyInput.x * speed), aRigidBody.velocity.y);
//        IsWallJumping = false;
//    }
//    else
//    {
//        aRigidBody.velocity = Vector2.Lerp(aRigidBody.velocity, (new Vector2(xyInput.x * speed, aRigidBody.velocity.y)), wallJumpLerp *Time.deltaTime);
//    }
//}
//private void Jump(Vector2 direction)
//{
//    aRigidBody.velocity = new Vector2(aRigidBody.velocity.x, 0);
//    aRigidBody.velocity += direction * jumpForce;
//}
//private void WallSlide()
//{
//    if(!IsAbleToMove)
//    {
//        return;
//    }
//    aRigidBody.velocity=new Vector2(aRigidBody.velocity.x, -slideSpeed);
//}
//private void ClimbWall()
//{
//    aRigidBody.velocity = Vector2.up * climbSpeed;
//}
//private void WallJump()
//{

//    Vector2 jumpDir;
//    if(aCollisions.IsOnRightWall)
//    {
//        jumpDir = Vector2.left;
//    }
//    else
//    {
//        jumpDir = Vector2.right;
//    }
//    StopCoroutine(DisableMovement(0));
//    StartCoroutine(DisableMovement(.1f));
//    Jump(Vector2.up / 1.5f + jumpDir / 1.5f);

//    IsWallJumping = true;
//}
//private void Dash(float x, float y)
//{
//    IsWallJumping = true;
//    aRigidBody.velocity = Vector2.zero;
//    aRigidBody.velocity += new Vector2(x, y).normalized * 30;
//}
//#endregion
//IEnumerator DisableMovement(float time)
//{
//    IsAbleToMove = false;
//    yield return new WaitForSeconds(time);
//    IsAbleToMove = true;
//}
