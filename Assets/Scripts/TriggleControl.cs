using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggleControl : MonoBehaviour
{
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    public void ResetTrigger(string triggerName)
    {
        _anim.ResetTrigger(triggerName);
    }
}
