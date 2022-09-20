using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableItemController : ItemController
{
    [SerializeField] private AudioClip onCollectAudioClip;

    protected override void OnCollect()
    {
        GameController.Instance.CollectableItemCollected(gameObject);
        GameController.Instance.PlaySound(onCollectAudioClip, transform.position);
        ProtectFromDestroy = true;
    }

}
