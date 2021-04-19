using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public float walkSpeed;
    public float runSpeed;
    public float jumpVelocity;
    public float rollVelocity;
    public float jabVelocity;
    public IUserInput pi;
    public bool _lockplanar = false;
    public CameraController camcon;

    [Header("====== Friction Settings ======")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    private Animator _anim;
    private Rigidbody _rigbody;
    private Vector3 _planarVector;
    private Vector3 _thrustVector; // push in vertical direction
    private bool _canAttack = true;
    private Collider _capsuleCollider;
    private float _lerpTarget;
    private Vector3 _deltaPos;
    // Start is called before the first frame update
    private void Awake()
    {
        IUserInput[] inputs = GetComponents<IUserInput>();
        foreach (var input in inputs)
        {
            if (input.enabled)
            {
                pi = input;
                break;
            }
        }
        _anim = model.GetComponent<Animator>();
        _rigbody = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    private void Update()
    {
        //press signal
        _anim.SetFloat("forward", Mathf.Lerp(_anim.GetFloat("forward"), pi.dMag * (pi.run ? 2.0f : 1.0f), 0.4f));
        _anim.SetBool("defense", pi.defense);

        if (pi.lockOn) camcon.LockUnLock();

        if (CheckAnimatorState("idle", "Attack")) //operate only when idle
        {
            if (pi.jump)
            {
                _anim.SetTrigger("jump");
                _canAttack = false;
            }
            if (pi.roll) _anim.SetTrigger("roll");
            if (pi.jab) _anim.SetTrigger("jab");
        }
        
        

        if (pi.attack && CheckAnimatorState("ground") && _canAttack) _anim.SetTrigger("attack");


        if (camcon.lockState == false)
        {
            if(pi.dMag > 0.01f)
            {
                Vector3 targetForward = Vector3.Slerp(model.transform.forward, pi.dVec, 0.3f);
                model.transform.forward = targetForward;
            }

            if (!_lockplanar)
            {
                _planarVector = pi.dMag * model.transform.forward * walkSpeed * (pi.run ? runSpeed : 1.0f);
            }
        }
        else
        {
            model.transform.forward = transform.forward;
            if (!_lockplanar) _planarVector = pi.dVec * walkSpeed * (pi.run ? runSpeed : 1.0f);
        }

            
    }

    private void FixedUpdate()
    {
        
        _rigbody.position += _deltaPos;
        //_rigbody.position += _planarVector * Time.fixedDeltaTime;
        _rigbody.velocity = new Vector3(_planarVector.x, _rigbody.velocity.y, _planarVector.z) + _thrustVector;
        _thrustVector = Vector3.zero;
        _deltaPos = Vector3.zero;
    }

    private bool CheckAnimatorState(string state, string layer = "Base Layer")
    {
        int layerIndex = _anim.GetLayerIndex(layer);
        return _anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(state);
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
        _capsuleCollider.material = frictionOne;
        _lockplanar = false;
        _canAttack = true;
        pi.inputEnabled = true;
    }

    public void OnFallingEnter()
    {
        _lockplanar = true;
        pi.inputEnabled = false;
    }

    public void OnRollEnter()
    {
        if (pi.dVec.magnitude > 0.1f) _planarVector = pi.dVec.normalized * rollVelocity;
        else _planarVector = model.transform.forward * rollVelocity;
        _lockplanar = true;
        pi.inputEnabled = false;
    }

    public void OnGroundExit()
    {
        _capsuleCollider.material = frictionZero;
    }

    public void OnJabEnter()
    {
        _planarVector = new Vector3(0, 0.5f * jumpVelocity, 0);
        _lockplanar = true;
        pi.inputEnabled = false;
    }

    public void OnJabUpdate()
    {
        _thrustVector = model.transform.forward * _anim.GetFloat("jabVelocity");
    }

    public void OnAttackIdleEnter()
    {
        //fdsgfsgd _anim.SetLayerWeight(_anim.GetLayerIndex("Attack"), 0f);
        _lerpTarget = 0f;
        pi.inputEnabled = true;
    }

    public void OnAttackIdleUpdate()
    {
        float currentWeight = _anim.GetLayerWeight(_anim.GetLayerIndex("Attack"));
        currentWeight = Mathf.Lerp(currentWeight, _lerpTarget, 0.2f);
        _anim.SetLayerWeight(_anim.GetLayerIndex("Attack"), currentWeight);
    }

    public void OnAttackEnter()
    {
        _lerpTarget = 1.0f;
        //_planarVector = model.transform.forward * rollVelocity;
        pi.inputEnabled = false;
    }

    public void OnAttackUpdate()
    {
        float currentWeight = _anim.GetLayerWeight(_anim.GetLayerIndex("Attack"));
        currentWeight = Mathf.Lerp(currentWeight, _lerpTarget, 0.2f);
        _anim.SetLayerWeight(_anim.GetLayerIndex("Attack"), currentWeight);
        _thrustVector = model.transform.forward * _anim.GetFloat("attack1hAVelocity");
    }

    public void UpdateRootMotion(object deltaPos)
    {
        if(CheckAnimatorState("attack1hC", "Attack"))
        {
            _deltaPos += (Vector3)deltaPos;
        }
        
    }
    #endregion
}
