using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstantItemController : ItemController
{
    public override void OnCollect()
    {
        StartEffect();
    }

}
