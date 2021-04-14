using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{

    public CapsuleCollider capsule;
    public float offset = -0.1f;

    private Vector3 _point1;
    private Vector3 _point2;
    private float _radius;

    private void Awake()
    {
        _radius = capsule.radius - 0.05f;
    }

    private void FixedUpdate()
    {
        _point1 = transform.position + transform.up * (_radius + offset);
        _point2 = transform.position + transform.up * (capsule.height - _radius + offset);

        Collider[] colliders = Physics.OverlapCapsule(_point1, _point2, _radius, LayerMask.GetMask("Ground"));
        if(colliders.Length > 0)
        {
            SendMessageUpwards("isOnGround");
        }
        else
        {
            SendMessageUpwards("NotOnGround");
        }


    }
}
