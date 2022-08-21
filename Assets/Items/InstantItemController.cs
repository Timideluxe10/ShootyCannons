using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstantItemController : ItemController
{
    protected override void OnCollect()
    {
        StartEffect();
    }

}
