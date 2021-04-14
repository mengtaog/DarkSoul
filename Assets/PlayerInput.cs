using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Variable
    [Header("====== Key Settings ======")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";
    public string keyRun;
    public string keyJump;


    [Header("====== Pressing Signals ======")]
    public bool run;

    [Header("====== Trigger Signals ======")]
    public bool jump;

    [Header("====== Output Signals ======")]
    public float dUp;
    public float dRight;
    public float dMag; 
    public Vector3 dVec; //人物目标朝向

    [Header("====== Others ======")]
    public bool inputEnabled = true;


    private float _targetDUp;
    private float _targetDRight;
    private float _dUpVelocity;
    private float _dRightVelocity;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
        
        dMag = Mathf.Sqrt(dUp2 * dUp2 + dRight2 * dRight2);
        dVec = transform.right * dRight2 + transform.forward * dUp2;

        run = Input.GetKey(keyRun);
        jump = Input.GetKeyDown(keyJump);
    }

    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output;
        output.x = input.x * Mathf.Sqrt(1.0f - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1.0f - (input.x * input.x) / 2.0f);
        return output;
    }
}
