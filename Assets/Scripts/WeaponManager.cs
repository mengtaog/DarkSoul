using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : IActorManagerInterface
{

    //public ActorManager am;
    

    public GameObject whL; //weapon handle left
    public GameObject whR; //weapon handle right
    public WeaponController wcL;
    public WeaponController wcR;


    private Collider _weaponColL;
    private Collider _weaponColR;


    // Start is called before the first frame update
    private void Start()
    {
        whL = transform.DeepFind("weaponHandleL").gameObject;
        whR = transform.DeepFind("weaponHandleR").gameObject;

        wcL = BindWeaponContrller(whL);
        wcR = BindWeaponContrller(whR);


        _weaponColL = whL.GetComponentInChildren<Collider>();
        _weaponColR = whR.GetComponentInChildren<Collider>();
    }

    public WeaponController BindWeaponContrller(GameObject targetObj)
    {
        WeaponController tmpInstance = targetObj.GetComponent<WeaponController>();
        if(tmpInstance == null)
        {
            tmpInstance = targetObj.AddComponent<WeaponController>();
        }
        tmpInstance.wm = this;
        return tmpInstance;
    }

    public void WeaponEnable()
    {
        if(am.ac.CheckAnimatorStateTag("attackL")) _weaponColL.enabled = true;
        else _weaponColR.enabled = true;
    }

    public void WeaponDisable()
    {
        _weaponColL.enabled = false;
        _weaponColR.enabled = false;
    }

    public void CounterBackEnable()
    {
        am.sm.SetIsCounterBack(true);
    }

    public void CounterBackDisable()
    {
        am.sm.SetIsCounterBack(false);
    }
}
