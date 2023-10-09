using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableJumpItem : CollectableItemController
{
    [SerializeField] private int force;
   
    protected override Effect SetEffect()
    {
        return new Jump(force);
    }

    public override string GetName()
    {
        return "Jump";
    }

}
