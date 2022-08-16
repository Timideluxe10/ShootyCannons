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

    private List<Vector3> predictionPoints = new List<Vector3>();
    private GameObject player;

    protected bool IsTurning { get => isTurning; }
    public int Points { get => points; }
    public int TimeBonus { get => timeBonus; }
    public bool IsVisited { get => isVisited; }
    public GameObject Player { get => player; }

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

            UpdatePredictionPoints();
        }
    }

    private void UpdatePredictionPoints()
    {
        predictionPoints = ShootingPrediction.Instance.GetPredictionPoints(transform.position + transform.up * 2, transform.up, 
            ((power * 100) / player.GetComponent<Rigidbody>().mass) * Time.fixedDeltaTime);
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

    private void OnDrawGizmos()
    {
        if (predictionPoints == null || predictionPoints.Count == 0 || !isTurning)
            return;
        Gizmos.color = Color.blue;
        foreach (Vector3 predictionPoint in predictionPoints)
        {
            Gizmos.DrawSphere(predictionPoint, .1f);
        }
    }


}
