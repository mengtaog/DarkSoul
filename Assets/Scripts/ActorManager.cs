using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{

    public ActorController ac;
    [Header("====== Auto Generate if Null ======")]
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;

    



    private void Awake()
    {
        GameObject sensor = transform.Find("sensor").gameObject;
        ac = GetComponent<ActorController>();
        GameObject model = ac.model;
        bm = Bind<BattleManager>(sensor);
        wm = Bind<WeaponManager>(model);
        sm = Bind<StateManager>(this.gameObject);

    }

    private T Bind<T>(GameObject obj) where T : IActorManagerInterface
    {
        T tempInstance = obj.GetComponent<T>();
        if(tempInstance == null)
        {
            tempInstance = obj.AddComponent<T>();
        }
        tempInstance.am = this;
        return tempInstance;

    }

    public void TryDoDamage(WeaponController targetWc)
    {
        if (sm.isImmortal)
        {
            //skip
        }
        else if (sm.canCounter)
        {
            targetWc.wm.am.Stunned();
            ac.IssueTrigger("counterBack");
        }
        else if (sm.isDefense) //when defense
        {
            Blocked();
        }
        else
        {
            if(sm.HP <= 0)
            {
                //already died
            }
            else
            {
                sm.AddHP(-5);
                if (sm.HP > 0) Hit();
                else Die();
            }
        }
    }

    public void Blocked()
    {
        ac.IssueTrigger("blocked");
    }

    public void Hit()
    {
        ac.IssueTrigger("hit");

    }

    public void Die()
    {
        ac.IssueTrigger("die");
        ac.pi.inputEnabled = false;
        if (ac.camcon.lockState) ac.camcon.LockUnLock();
        ac.camcon.enabled = false;
    }

    public void Stunned()
    {
        ac.IssueTrigger("stunned");
        ac.pi.inputEnabled = false;
    }
}
