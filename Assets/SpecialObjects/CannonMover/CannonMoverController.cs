using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMoverController : MonoBehaviour
{
    [SerializeField] private GameObject mountedCannonTemplate;
    private GameObject mountedCannon;

    [SerializeField] private float speed;

    private bool isGoingForward = true;
    private float length;

    private void Start()
    {
        length = transform.localScale.x;
        Vector3 offsetFromCenter = transform.right * (length / 2);
        mountedCannon = GameObject.Instantiate(mountedCannonTemplate, transform.position - offsetFromCenter, mountedCannonTemplate.transform.rotation);
    }

    private void Update()
    {
        int multiplier = isGoingForward ? 1 : -1;
        mountedCannon.transform.position += transform.right * speed * multiplier * Time.deltaTime;
        if (Vector3.Distance(transform.position, mountedCannon.transform.position) >= length / 2)
            isGoingForward = !isGoingForward;
    }
}
