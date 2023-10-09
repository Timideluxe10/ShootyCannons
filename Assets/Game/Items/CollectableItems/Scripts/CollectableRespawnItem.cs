using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRespawnItem : CollectableItemController
{
    protected override Effect SetEffect()
    {
        return new Respawn();
    }

    public override string GetName()
    {
        return "Respawn";
    }

}
