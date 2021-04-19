using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    //[Header("====== Key Settings ======")]
    //public string keyUp = "w";
    //public string keyDown = "s";
    //public string keyLeft = "a";
    //public string keyRight = "d";
    //public string keyAttack;
    //public string keyDefense;
    //public string keyRun;
    //public string keyJump;
    //public string keyRoll;
    //public string keyJab;
    //public string keyJRight;
    //public string keyJLeft;
    //public string keyJUp;
    //public string keyJDown;

    [Header("====== Pressing Signals ======")]
    public bool run;
    public bool defense;

    [Header("====== Trigger Signals ======")]
    public bool jump;
    public bool roll;
    public bool jab;
    public bool attack;
    public bool lockOn;

    [Header("====== Output Signals ======")]
    public float dUp;
    public float dRight;
    public float dMag;
    public Vector3 dVec; //人物目标朝向

    public float jUp;
    public float jRight;

    [Header("====== Others ======")]
    public bool inputEnabled = true;


    protected float _targetDUp;
    protected float _targetDRight;
    protected float _dUpVelocity;
    protected float _dRightVelocity;

    protected Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output;
        output.x = input.x * Mathf.Sqrt(1.0f - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1.0f - (input.x * input.x) / 2.0f);
        return output;
    }
}
