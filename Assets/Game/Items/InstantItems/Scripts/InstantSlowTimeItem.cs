using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantSlowTimeItem : InstantItemController
{
    protected override Effect SetEffect()
    {
        return new SlowTime();
    }
    public override string GetName()
    {
        return "Slow Motion";
    }

}
