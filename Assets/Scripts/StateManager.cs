using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : IActorManagerInterface
{
  
    public float HPMax = 15.0f;
    public float HP = 15.0f;

    [Header("====== 1st order state flag ======")]
    public bool isGround;
    public bool isJump;
    public bool isRoll;
    public bool isFall;
    public bool isJab;
    public bool isAttack;
    public bool isHit;
    public bool isBlocked;
    public bool isDie;
    public bool isCounterBack = false;


    [Header("====== 2nd order state flag ======")]
    public bool isAllowDefense;
    public bool isImmortal;
    public bool isDefense;
    public bool canCounter = false;

    private void Start()
    {
        HP = HPMax;
    }

    private void Update()
    {
        //1st
        isGround = am.ac.CheckAnimatorState("ground");
        isJump = am.ac.CheckAnimatorState("jump");
        isFall = am.ac.CheckAnimatorState("falling");
        isRoll = am.ac.CheckAnimatorState("roll");
        isJab = am.ac.CheckAnimatorState("jab");
        isAttack = am.ac.CheckAnimatorStateTag("attackL") || am.ac.CheckAnimatorStateTag("attackR");
        isHit = am.ac.CheckAnimatorState("hit");
        isBlocked = am.ac.CheckAnimatorState("blocked");
        isDie = am.ac.CheckAnimatorState("die");
        //isCounterBack = am.ac.CheckAnimatorState("counterBack");

        //2nd
        isAllowDefense = isGround || isBlocked;
        isDefense = isAllowDefense && am.ac.CheckAnimatorState("defense1h","Defense");
        isImmortal = isRoll || isJab;
    }

    public void AddHP(float value)
    {
        HP += value;
        Mathf.Clamp(HP, 0, HPMax);
        
    }  

    public void TryCounterBack()
    {
        if (canCounter) return;
        canCounter = true;
        StartCoroutine(CounterBackCounter());
    }

    IEnumerator CounterBackCounter()
    {
        yield return new WaitForSeconds(0.1f);
        canCounter = false;
    }

    public void SetIsCounterBack(bool value)
    {
        isCounterBack = value; 
    }
}
