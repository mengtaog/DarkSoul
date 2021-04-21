using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Collider weaponCol;
    
    // Start is called before the first frame update
    public void WeaponEnable()
    {
        weaponCol.enabled = true;
    }

    public void WeaponDisable()
    {
        weaponCol.enabled = false;
    }
}
