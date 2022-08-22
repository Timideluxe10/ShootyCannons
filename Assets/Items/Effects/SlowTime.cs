using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : Effect
{
    public override void Start()
    {
        GameController.Instance.SetTimeScale(0.7f);
    }

    public override void Stop()
    {
        GameController.Instance.SetTimeScale(1f);
    }
}
