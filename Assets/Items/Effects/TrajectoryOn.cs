using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryOn : Effect
{
    public override void Start()
    {
        GameController.Instance.SetDrawTrajectory(true);
    }
    public override void Stop()
    {
        GameController.Instance.SetDrawTrajectory(false);
    }
}
