using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmAnimFix : MonoBehaviour
{
    public Vector3 v;

    private Animator _anim;
    private Transform _leftLowerArm;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        
    }

    private void OnAnimatorIK()
    {
        _leftLowerArm = _anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
        _leftLowerArm.eulerAngles += v;
        _anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(_leftLowerArm.eulerAngles));
        //print("!!");
    }


}
