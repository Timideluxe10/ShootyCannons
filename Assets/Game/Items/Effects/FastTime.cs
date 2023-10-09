using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTime : Effect
{
    public override void Start()
    {
        GameController.Instance.SetTimeScale(1.7f);
    }
    public override void Stop()
    {
        GameController.Instance.SetTimeScale(1f);
    }
    public override EffectType GetEffectType()
    {
        return EffectType.TIME;
    }

}
