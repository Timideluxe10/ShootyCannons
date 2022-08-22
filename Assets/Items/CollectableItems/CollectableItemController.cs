using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableItemController : ItemController
{
    protected override void OnCollect()
    {
        GameController.Instance.CollectableItemCollected(gameObject);
        ProtectFromDestroy = true;
    }

}
