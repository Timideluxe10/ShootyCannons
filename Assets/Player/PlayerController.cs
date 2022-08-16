using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float startingTime;

    private int points;
    private float time;
    private GameObject currentCannon;

    private new Rigidbody rigidbody;
    private MeshRenderer meshRenderer;

    public int Points { get => points; }
    public GameObject CurrentCannon { get => currentCannon; set => currentCannon = value; }

    // Start is called before the first frame update
    void Start()
    {
        time = startingTime;
        rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Shooting
        if(currentCannon != null)
        {
            if (Input.GetKeyDown(InputController.SHOOT))
            {
                currentCannon.GetComponent<CannonController>().Shoot();
            }
        }
    }

    public void OnCannonEnter(GameObject cannon)
    {
        // Deactivate player and set its position to the center of the cannon.
        SetActive(false);
        transform.position = cannon.transform.position;

        // Update variables.
        currentCannon = cannon;
        CannonController cannonController = cannon.GetComponent<CannonController>();
        if(!cannonController.IsVisited)
        {
            points += cannonController.Points;
            time += cannonController.TimeBonus;
        }

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
        currentCannon = null;
        SetActive(true);
    }
}
