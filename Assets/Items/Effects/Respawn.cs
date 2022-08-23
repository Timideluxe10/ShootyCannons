using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : Effect
{
    public override void Start()
    {
        GameObject player = GameController.Instance.Player;
        PlayerController playerController = player.GetComponent<PlayerController>();
        if(playerController.CurrentCannon == null && playerController.LastCannon != null)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.transform.position = playerController.LastCannon.transform.position + new Vector3(0, 3, 0);
        }
    }
    public override void Stop()
    {
        return;
    }
    public override string GetName()
    {
        return "Respawn";
    }
}
