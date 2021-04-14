using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public float walkSpeed;
    public float runSpeed;
    public float jumpVelocity = 3.0f;
    public float rollVelocity = 4.0f;
    public PlayerInput pi;


    private Animator _anim;
    private Rigidbody _rigbody;
    private Vector3 _planarVector;
    private Vector3 _thrustVector; // push in vertical direction
    public bool _lockplanar = false;
    // Start is called before the first frame update
    private void Awake()
    {
        pi = GetComponent<PlayerInput>();
        _anim = model.GetComponent<Animator>();
        _rigbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        //animation
        _anim.SetFloat("forward", Mathf.Lerp(_anim.GetFloat("forward"), pi.dMag * (pi.run ? 2.0f : 1.0f), 0.4f));
        if(pi.jump) _anim.SetTrigger("jump");

        if(pi.dMag > 0.01f)
        {
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.dVec, 0.3f);
            model.transform.forward = targetForward;
        }

        if(_rigbody.velocity.magnitude > 0f)
        {
            _anim.SetTrigger("roll");
        }

        if (!_lockplanar)
        {
            _planarVector = pi.dMag * model.transform.forward * walkSpeed * (pi.run ? runSpeed : 1.0f);
        }
            
    }

    private void FixedUpdate()
    {
        //_rigbody.position += _planarVector * Time.fixedDeltaTime;
        _rigbody.velocity = new Vector3(_planarVector.x, _rigbody.velocity.y, _planarVector.z) + _thrustVector;
        _thrustVector = Vector3.zero;
    }

    #region handle messages

    public void OnJumpEnter()
    {
        _thrustVector = new Vector3(0, jumpVelocity, 0);
        _lockplanar = true;
        pi.inputEnabled = false;
    }

    public void OnJumpExit()
    {
        
    }

    public void isOnGround()
    {
        _anim.SetBool("isOnGround", true);
    }

    public void NotOnGround()
    {
        _anim.SetBool("isOnGround", false);
    }

    public void OnGroundEnter()
    {
        _lockplanar = false;
        pi.inputEnabled = true;
    }

    public void OnFallingEnter()
    {
        _lockplanar = true;
        pi.inputEnabled = false;
    }

    public void OnRollEnter()
    {
        _thrustVector = new Vector3(0, rollVelocity, 0);
        _lockplanar = true;
        pi.inputEnabled = false;
    }
    #endregion
}
