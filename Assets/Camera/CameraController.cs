using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private int distance;
    [SerializeField] float smoothness;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, 
            player.transform.position - new Vector3(0, 0, this.distance),
            this.smoothness * Time.deltaTime);
    }
}
