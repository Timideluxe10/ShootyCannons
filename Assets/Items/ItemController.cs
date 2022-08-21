using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemController : MonoBehaviour
{
    [SerializeField] protected float duration;
    private float durationLeft;

    private Effect effect;
    private bool isEffectActive = false;

    public float Duration { get => duration; }

    // Start is called before the first frame update
    protected void Start()
    {
        effect = SetEffect();
        durationLeft = duration;
    }

    protected abstract Effect SetEffect();

    // Update is called once per frame
    protected void Update()
    {
        if (isEffectActive)
        {
            durationLeft -= Time.deltaTime;
            if(durationLeft < 0)
            {
                StopEffect();
            }
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        GameObject otherGameObject = otherCollider.gameObject;
        if (otherGameObject.CompareTag("Player"))
        {
            OnCollect();
        }
    }

    protected abstract void OnCollect();

    public void StartEffect()
    {
        effect.Start();
        isEffectActive = true;
    }

    public void StopEffect()
    {
        effect.Stop();
        GameObject.Destroy(this);
    }
}
