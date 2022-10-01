using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantFastTimeItem : InstantItemController
{
    protected override Effect SetEffect()
    {
        return new FastTime();
    }
}
