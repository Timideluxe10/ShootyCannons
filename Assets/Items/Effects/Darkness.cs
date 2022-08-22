using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darkness : Effect
{
    private GameObject lightObject;
    public override void Start()
    {
        lightObject = GameObject.FindGameObjectWithTag("Light");
        lightObject.SetActive(false);
    }

    public override void Stop()
    {
        lightObject.SetActive(true);

    }
}
