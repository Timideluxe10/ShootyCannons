using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableTrajectoryOnItem : CollectableItemController
{
    protected override Effect SetEffect()
    {
        return new TrajectoryOn();
    }
}
