using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDarknessItem : InstantItemController
{
    protected override Effect SetEffect()
    {
        return new Darkness();
    }
}
