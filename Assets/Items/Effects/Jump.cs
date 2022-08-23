using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Effect
{
    private int force;

    public Jump(int force)
    {
        this.force = force;
    }

    public override void Start()
    {
        GameObject player = GameController.Instance.Player;
        PlayerController playerController = player.GetComponent<PlayerController>();
        if(playerController.CurrentCannon == null)
        {
            Rigidbody rigidboy = player.GetComponent<Rigidbody>();
            rigidboy.velocity -= new Vector3(0, rigidboy.velocity.y, 0);
            rigidboy.AddForce(new Vector3(0, force, 0), ForceMode.Impulse);
        }
    }
    public override void Stop()
    {
        return;
    }
    public override string GetName()
    {
        return "Jump";
    }
    public override EffectType GetEffectType()
    {
        return EffectType.INSTANT;
    }

}
