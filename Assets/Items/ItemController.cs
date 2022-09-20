using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemController : MonoBehaviour
{
    private static readonly float Turn_Speed = 2f;

    [SerializeField] protected float duration;
    private float durationLeft;

    private Effect effect;
    private bool isEffectActive = false;

    private bool protectFromDestroy = false;

    [SerializeField] private AudioClip effectAudioClip;

    public float MaxDuration { get => duration; }
    public bool ProtectFromDestroy { get => protectFromDestroy; set => protectFromDestroy = value; }
    public float DurationLeft { get => durationLeft; }

    // Start is called before the first frame update
    protected void Start()
    {
        effect = SetEffect();
        durationLeft = duration;
    }

    protected abstract Effect SetEffect();

    public Effect.EffectType GetEffectType()
    {
        return effect.GetEffectType();
    }

    public string GetEffectName()
    {
        return effect.GetName();
    }

    // Update is called once per frame
    protected void Update()
    {

        transform.RotateAround(transform.position, Vector3.forward, -Time.deltaTime * 50 * Turn_Speed);

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
            Disable();
            OnCollect();
        }
    }

    private void Disable()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    protected abstract void OnCollect();

    public void StartEffect()
    {
        GameController.Instance.OnItemUse(this);
        if (effectAudioClip != null)
            GameController.Instance.PlaySound(effectAudioClip, transform.position);
        effect.Start();
        isEffectActive = true;
        protectFromDestroy = true;
    }

    public void StopEffect()
    {
        GameController.Instance.OnItemExpire(this);
        effect.Stop();
        GameObject.Destroy(gameObject);
    }
}
