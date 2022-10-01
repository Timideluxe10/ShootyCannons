using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSlowTimeItem : CollectableItemController
{
    protected override Effect SetEffect()
    {
        return new SlowTime();
    }
}
