using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public abstract void Start();

    public abstract void Stop();

    public abstract string GetName();
}
