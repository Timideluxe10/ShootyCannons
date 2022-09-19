using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour, Generateable
{
    [SerializeField] private float speed;
    [SerializeField] private float power;
    [SerializeField] private float timeBonus;

    private bool isTurning = false;
    private bool isVisited = false;

    private GameObject player;

    public bool IsVisited { get => isVisited; }
    public float Power { get => power; }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isTurning)
        {
            transform.Rotate(Vector3.forward * speed * 50 * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        GameObject otherGameObject = otherCollider.gameObject;
        if (otherGameObject.CompareTag("Player"))
        {
            // Inform player
            player = otherGameObject;
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.OnCannonEnter(gameObject);

            // Update variables
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
        isTurning = false;
        player.transform.position = transform.position + transform.up * 2;
        player.GetComponent<PlayerController>().OnCannonExit();
        player.GetComponent<Rigidbody>().AddForce(transform.up * power * 100);
        GameController.Instance.PlaySound(GetComponent<AudioSource>().clip, transform.position);
    }

    public GameObject GetCannonToGenerateFrom()
    {
        return gameObject;
    }
}
