using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantTrajectoryOnItem : InstantItemController
{
    protected override Effect SetEffect()
    {
        return new TrajectoryOn();
    }
}