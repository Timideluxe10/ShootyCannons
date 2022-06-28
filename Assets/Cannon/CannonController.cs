using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    private bool isTurning = false;
    private bool isVisited = false;

    [SerializeField] private float speed;
    [SerializeField] private float power;
    [SerializeField] private readonly int points;
    [SerializeField] private readonly int timeBonus;

    protected bool IsTurning { get => isTurning; }
    public int Points { get => points; }
    public int TimeBonus { get => timeBonus; }
    public bool IsVisited { get => isVisited; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if(isTurning)
        {
            transform.Rotate(Vector3.forward * speed);
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        GameObject otherGameObject = otherCollider.gameObject;
        if (otherGameObject.CompareTag("Player"))
        {
            PlayerController playerController = otherGameObject.GetComponent<PlayerController>();
            playerController.OnCannonEnter(gameObject);
            isTurning = true;
            isVisited = true;
        }
    }

    public void Shoot(GameObject player)
    {
        // GetComponent<AudioSource>().Play();
        player.transform.position = gameObject.transform.position + gameObject.transform.up * 2;
        player.GetComponent<PlayerController>().OnCannonExit();
        player.GetComponent<Rigidbody>().AddForce(gameObject.transform.up * this.power * 100);
        isTurning = false;
    }


}
