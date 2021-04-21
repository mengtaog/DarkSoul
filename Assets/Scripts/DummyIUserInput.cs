using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyIUserInput : IUserInput
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        attack = true;
        yield return 0;
    }

    private void Update()
    {
        UpdateDmagDvec(dUp, dRight);
    }
}
