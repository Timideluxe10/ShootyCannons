using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public enum EffectType
    {
        TIME, GRAVITY, TRAJECTORY, LIGHT, INSTANT
    }

    public abstract EffectType GetEffectType();

    public abstract void Start();

    public abstract void Stop();

    public abstract string GetName();
}
