using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : MonoBehaviour
{
    public ActorManager am;

    private CapsuleCollider _defCol;
    private void Start()
    {
        _defCol = GetComponent<CapsuleCollider>();
        _defCol.center = Vector3.up * 1.0f;
        _defCol.height = 2.0f;
        _defCol.radius = 0.25f;
        _defCol.isTrigger = true;
    }


    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Weapon")
        {
            am.DoDamage();
        }
    }

}
