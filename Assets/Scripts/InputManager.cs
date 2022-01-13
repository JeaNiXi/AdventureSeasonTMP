using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private MainCharacter mainCharacterScript;


    //enum CharacterState { IS_GROUNDED, IS_IN_AIR };
    //[SerializeField] CharacterState mcState = CharacterState.IS_GROUNDED;
    void Start()
    {
        //SetupCom();
    }

    private void SetupCom()
    {
        //mainCharacterScript = GameObject.FindGameObjectWithTag("Player").GetComponent<MainCharacter>();
    }

    void Update()
    {
        //CheckInput();
    }

    private void FixedUpdate()
    {

    }

    //private void CheckInput()
    //{
    //    float moveInput = Input.GetAxisRaw("Horizontal");
    //    //float heightInput = Input.GetAxisRaw("Jump");
    //    if (moveInput > 0)
    //    {
    //        mainCharacterScript.Move(Vector3.right, false);
    //        return;
    //    }
    //    else if (moveInput < 0)
    //    {
    //        mainCharacterScript.Move(Vector3.left, false);
    //        return;
    //    }
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        mainCharacterScript.Jump();
    //    }
    //    //if (Input.GetKey(KeyCode.D))
    //    //{
    //    //    mainCharacterScript.Move(Vector3.right, false);
    //    //    return;
    //    //}
    //    //if (Input.GetKey(KeyCode.A))
    //    //{
    //    //    mainCharacterScript.Move(Vector3.left, false);
    //    //    return;
    //    //}
    //    //mainCharacterScript.Move(Vector3.zero, false);
    //    //mcState = CharacterState.IS_GROUNDED;
    //}
}
