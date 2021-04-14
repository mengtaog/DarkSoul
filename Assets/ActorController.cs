using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public float walkSpeed;
    public float runSpeed;

    private PlayerInput _pi;
    private Animator _anim;
    private Rigidbody _rigbody;
    private Vector3 _movingVector;
    // Start is called before the first frame update
    void Awake()
    {
        _pi = GetComponent<PlayerInput>();
        _anim = model.GetComponent<Animator>();
        _rigbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //animation
        _anim.SetFloat("forward", Mathf.Lerp(_anim.GetFloat("forward"), _pi.dMag * (_pi.run ? 2.0f : 1.0f), 0.4f));
        if(_pi.jump) _anim.SetTrigger("jump");

        if(_pi.dMag > 0.01f)
        {
            Vector3 targetForward = Vector3.Slerp(model.transform.forward, _pi.dVec, 0.3f);
            model.transform.forward = targetForward;
        }
            
        _movingVector = _pi.dMag * model.transform.forward * walkSpeed * (_pi.run ? runSpeed : 1.0f);
    }

    private void FixedUpdate()
    {
        _rigbody.position += _movingVector * Time.fixedDeltaTime;
    }
}
