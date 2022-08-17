using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject debug;

    [SerializeField] private bool doDebug;

    private GameController() { instance = this; }
    public static GameController Instance { get => instance == null ? (instance = new GameController()) : instance; }

    public GameObject Player { get => player; }

    public PlayerController GetPlayerController()
    {
        return player.GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!doDebug)
            debug.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
