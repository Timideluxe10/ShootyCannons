using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    private bool isTurning = false;
    private bool isVisited = false;

    [SerializeField] private float speed;
    [SerializeField] private float power;
    [SerializeField] private float timeBonus;

    private GameObject player;

    public bool IsTurning { get => isTurning; }
    public float TimeBonus { get => timeBonus; }
    public bool IsVisited { get => isVisited; }
    public GameObject Player { get => player; }
    public float Power { get => power; }

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
            player = otherGameObject;
            PlayerController playerController = otherGameObject.GetComponent<PlayerController>();
            playerController.OnCannonEnter(gameObject);
            isTurning = true;
            if (!isVisited)
                GameController.Instance.ProcessBonusTime(timeBonus);
            isVisited = true;
        }
    }

    public void Shoot()
    {
        if (player == null)
            return;
        // GetComponent<AudioSource>().Play();
        isTurning = false;
        player.transform.position = transform.position + transform.up * 2;
        player.GetComponent<PlayerController>().OnCannonExit();
        player.GetComponent<Rigidbody>().AddForce(transform.up * power * 100);
    }
}
