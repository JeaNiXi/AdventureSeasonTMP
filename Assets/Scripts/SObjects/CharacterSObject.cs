using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Character", menuName = "CharacterSO")]

public class CharacterSObject : ScriptableObject
{
    [SerializeField] private string _name;




    [SerializeField] private float _speed;




    public string Name
    {
        get
        {
            return _name;
        }
    }





    public float Speed
    {
        get
        {
            return _speed;
        }
    }

}
