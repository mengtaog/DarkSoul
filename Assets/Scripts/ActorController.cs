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
    private bool _lockplanar = false; //锁定速度
    private bool _trackDirection = false; //锁定方向
    //private bool _rightHand = true;
    private bool _leftShield = true;
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
        //
        if (camcon.lockState == false)
        {
            _anim.SetFloat("forward", Mathf.Lerp(_anim.GetFloat("forward"), pi.dMag * (pi.run ? 2.0f : 1.0f), 0.4f));
            _anim.SetFloat("right", 0f);
        }
        else
        {
            Vector3 localVector = transform.InverseTransformVector(pi.dVec);
            _anim.SetFloat("forward", localVector.z * (pi.run ? 2.0f : 1.0f));
            //print(pi.dVec.x);
            _anim.SetFloat("right", localVector.x * (pi.run ? 2.0f : 1.0f));
        }
        _anim.SetBool("defense", pi.defense);

        if (pi.lockOn) camcon.LockUnLock();

        /*
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
        */
        if (pi.jump)
        {
            _anim.SetTrigger("jump");
            _canAttack = false;
        }
        if (pi.roll) _anim.SetTrigger("roll");
        if (pi.jab) _anim.SetTrigger("jab");

        if ((pi.attack || pi.leftAttack) && (CheckAnimatorState("ground") || CheckAnimatorStateTag("attack")) && _canAttack)
        {
            if (pi.leftAttack) _anim.SetBool("R0L1", true);
            else _anim.SetBool("R0L1", false);
            _anim.SetTrigger("attack");
        }

        if (CheckAnimatorState("ground") && _leftShield)
        {
            if(pi.defense)
                _anim.SetLayerWeight(_anim.GetLayerIndex("Defense"), 1.0f);
            else _anim.SetLayerWeight(_anim.GetLayerIndex("Defense"), 0f);
        }
        else _anim.SetLayerWeight(_anim.GetLayerIndex("Defense"), 0f);

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
            if(!_trackDirection) model.transform.forward = transform.forward;
            else
            {
                model.transform.forward = _planarVector.normalized;
            }
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

    private bool CheckAnimatorStateTag(string tag, string layer = "Base Layer")
    {
        int layerIndex = _anim.GetLayerIndex(layer);
        return _anim.GetCurrentAnimatorStateInfo(layerIndex).IsTag(tag);
    }

    #region handle messages
    public void OnJumpEnter()
    {
        _thrustVector = new Vector3(0, jumpVelocity, 0);
        _lockplanar = true;
        pi.inputEnabled = false;
        _trackDirection = true;
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
        _trackDirection = false;
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
        _trackDirection = true;
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


    public void OnAttackEnter()
    {
        //_lerpTarget = 1.0f;
        //_planarVector = model.transform.forward * rollVelocity;
        pi.inputEnabled = false;
    }

    public void OnAttackUpdate()
    {
        //float currentWeight = _anim.GetLayerWeight(_anim.GetLayerIndex("Attack"));
        //currentWeight = Mathf.Lerp(currentWeight, _lerpTarget, 0.2f);
        //_anim.SetLayerWeight(_anim.GetLayerIndex("Attack"), currentWeight);
        _thrustVector = model.transform.forward * _anim.GetFloat("attack1hAVelocity");
    }

    public void UpdateRootMotion(object deltaPos)
    {
        if(CheckAnimatorState("attack1hC"))
        {
            _deltaPos += (Vector3)deltaPos;
        }
        
    }

    public void OnHitEnter()
    {
        _planarVector = Vector3.zero;
        pi.inputEnabled = false;

    }

    public void IssueTrigger(string triggerName)
    {
        _anim.SetTrigger(triggerName);
    }
    #endregion

   
}
