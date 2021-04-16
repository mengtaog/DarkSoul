using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        Vector3 deltaPos = _anim.deltaPosition;
        SendMessageUpwards("UpdateRootMotion", deltaPos);


    }
}
