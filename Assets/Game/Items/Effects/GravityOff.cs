using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityOff : Effect
{
    private Vector3 normalGravity;

    public override void Start()
    {
        normalGravity = Physics.gravity;
        Physics.gravity = Vector3.zero;
    }

    public override void Stop()
    {
        Physics.gravity = normalGravity;
    }
    public override string GetName()
    {
        return "Disable Gravity";
    }
    public override EffectType GetEffectType()
    {
        return EffectType.GRAVITY;
    }

}
