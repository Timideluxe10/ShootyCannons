using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantGravityOffItem : InstantItemController
{
    protected override Effect SetEffect()
    {
        return new GravityOff();
    }
    public override string GetName()
    {
        return "Gravity Off";
    }

}
