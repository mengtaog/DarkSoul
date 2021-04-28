using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : IUserInput
{
    
   [Header("====== Key Settings ======")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";
    public string keyAttack;
    public string keyLeftAttck;
    public string keyDefense;
    public string keyRun;
    public string keyJump;
    public string keyRoll;
    public string keyLockOn;
    public string keyJab;
    public string keyJRight;
    public string keyJLeft;
    public string keyJUp;
    public string keyJDown;

    [Header("====== Mouse settings ======")]
    public bool mouseEnable = false;
    public float mouseSensitivityX = 1.0f;
    public float mouseSensitivityY = 1.0f;

    //[Header("====== Pressing Signals ======")]
    //public bool run;

    //[Header("====== Trigger Signals ======")]
    //public bool jump;
    //public bool roll;
    //public bool jab;
    //public bool attack;

    //[Header("====== Output Signals ======")]
    //public float dUp;
    //public float dRight;
    //public float dMag; 
    //public Vector3 dVec; //人物目标朝向

    //public float jUp;
    //public float jRight;

    //[Header("====== Others ======")]
    //public bool inputEnabled = true;


    //private float _targetDUp;
    //private float _targetDRight;
    //private float _dUpVelocity;
    //private float _dRightVelocity;


    // Update is called once per frame
    void Update()
    {
        if (mouseEnable)
        {
            jUp = Input.GetAxis("Mouse Y") * mouseSensitivityY;
            jRight = Input.GetAxis("Mouse X") * mouseSensitivityX;
        }
        else
        {
            jUp = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown) ? 1.0f : 0);
            jRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);

        }
        _targetDUp = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        _targetDRight = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (!inputEnabled)
        {
            _targetDUp = 0;
            _targetDRight = 0;
        }

        

        dUp = Mathf.SmoothDamp(dUp, _targetDUp, ref _dUpVelocity, 0.1f);
        dRight = Mathf.SmoothDamp(dRight, _targetDRight, ref _dRightVelocity, 0.1f);

        Vector2 tmpD = SquareToCircle(new Vector2(dUp, dRight));
        
        float dUp2 = tmpD.x;
        float dRight2 = tmpD.y;

        UpdateDmagDvec(dUp2, dRight2);

        run = Input.GetKey(keyRun);
        jump = Input.GetKeyDown(keyJump);
        roll = Input.GetKeyDown(keyRoll);
        jab = Input.GetKeyDown(keyJab);
        attack = Input.GetKeyDown(keyAttack);
        leftAttack = Input.GetKeyDown(keyLeftAttck);
        defense = Input.GetKey(keyDefense);
        lockOn = Input.GetKeyDown(keyLockOn);
        tryCounterBack = Input.GetKeyDown(keyDefense);
    }

    //private Vector2 SquareToCircle(Vector2 input)
    //{
    //    Vector2 output;
    //    output.x = input.x * Mathf.Sqrt(1.0f - (input.y * input.y) / 2.0f);
    //    output.y = input.y * Mathf.Sqrt(1.0f - (input.x * input.x) / 2.0f);
    //    return output;
    //}
}
