using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableItemController : ItemController
{
    protected override void OnCollect()
    {
        GameController.Instance.CollectableItemCollected(gameObject);
        GameController.Instance.PlaySound(GetComponent<AudioSource>().clip, transform.position);
        ProtectFromDestroy = true;
    }

}
