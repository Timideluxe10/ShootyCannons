using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int points;
    private GameObject currentCannon;
    private GameObject lastCannon;

    private new Rigidbody rigidbody;
    private MeshRenderer meshRenderer;

    public int Points { get => points; }
    public GameObject CurrentCannon { get => currentCannon; }
    public GameObject LastCannon { get => lastCannon; }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void TryShoot()
    {
        if (currentCannon != null)
        {
            currentCannon.GetComponent<CannonController>().Shoot();
        }
    }

    public void OnCannonEnter(GameObject cannon)
    {
        // Deactivate player and set its position to the center of the cannon.
        SetActive(false);
        transform.position = cannon.transform.position;

        // Update variables.
        currentCannon = cannon;
    }

    private void SetActive(bool active)
    {
        rigidbody.isKinematic = !active;
        meshRenderer.enabled = active;
        if (!active)
            rigidbody.velocity = Vector3.zero;
    }

    public void OnCannonExit()
    {
        lastCannon = currentCannon;
        currentCannon = null;
        SetActive(true);
    }
}
