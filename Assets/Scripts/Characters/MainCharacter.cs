using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{


    private Animator mcAnimator;
    private Rigidbody2D mcRigidbody2D;
    private BoxCollider2D mcBoxCollider2D;

    [SerializeField] private bool _isGrounded;

    bool test;
    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            _isGrounded = value;
        }
    }


    void Start()
    {
        SetupCom();
    }

    private void SetupCom()
    {
        mcAnimator = gameObject.GetComponent<Animator>();
        mcRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        mcBoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }
    private void CheckInput()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float heightInput = Input.GetAxisRaw("Jump");
        if (moveInput > 0 && !test)
        {
            Move(Vector3.right, false);
            return;
        }
        else if (moveInput < 0 && !test)
        {
            Move(Vector3.left, false);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            test = true;
            Debug.Log("Jumped");
            mcRigidbody2D.AddForce(Vector2.up*1500f*Time.deltaTime,ForceMode2D.Impulse);
            mcAnimator.SetBool("_isGrounded", false);
            _isGrounded = false;
            //mcRigidbody2D.AddForce(Vector2.up * 50f * Time.deltaTime,ForceMode2D.Impulse);
        }
        if (IsGrounded && !test)
        {
            Move(Vector3.zero, false);
        }
    }
    private void FixedUpdate()
    {

    }
    public void Move(Vector3 direction, bool jump)
    {
        if (IsGrounded)
        {
            mcAnimator.SetBool("_isGrounded", true);
            switch (direction.x)
            {
                case 1:

                    gameObject.transform.Translate(direction * 6.0f * Time.deltaTime);
                    mcAnimator.SetFloat("_speed", 1);
                    FlipAnimation(false);
                    break;
                case -1:
                    gameObject.transform.Translate(direction * 6.0f * Time.deltaTime);
                    mcAnimator.SetFloat("_speed", 1);
                    FlipAnimation(true);
                    break;
                default:
                    mcAnimator.SetFloat("_speed", 0);
                    break;
            }
        }

    }




    private void FlipAnimation(bool isFlipped)
    {
        if(isFlipped)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = Vector3.one;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
            test = false;
        }
        else
        {
            IsGrounded = false;
            test = true;
        }
    }
}
