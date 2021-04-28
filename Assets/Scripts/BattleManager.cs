using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : IActorManagerInterface
{
    //public ActorManager am;

    private CapsuleCollider _defCol;
    private void Start()
    {
        _defCol = GetComponent<CapsuleCollider>();
        _defCol.center = Vector3.up * 1.0f;
        _defCol.height = 2.0f;
        _defCol.radius = 0.35f;
        _defCol.isTrigger = true;
    }


    private void OnTriggerEnter(Collider col)
    {
        WeaponController targetWc = col.GetComponentInParent<WeaponController>();

        GameObject attacker = targetWc.wm.am.gameObject;
        GameObject receiver = this.am.gameObject;

        Vector3 attackDir = receiver.transform.position - attacker.transform.position;
        float attackAngle = Vector3.Angle(attacker.transform.forward, attackDir);

        if(col.tag == "Weapon")
        {
            if(attackAngle <= 60f) am.TryDoDamage(targetWc);
        }
    }



}
