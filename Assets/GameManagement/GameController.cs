using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        RUNNING, PAUSED
    }
    private GameState gameState;

    private static GameController instance;

    private static readonly Vector3 Position_Player_Spawn = new Vector3(0, 3, 0);

    [SerializeField] private GameObject playerTemplate;
    private GameObject player;

    [SerializeField] private GameObject debug;
    [SerializeField] private bool doDebug;

    [SerializeField] private GameObject timeManagement;
    private TimeManager timeManager;

    [SerializeField] private GameObject gameOverManagement;
    private GameOverManager gameOverManager;

    [SerializeField] private GameObject pauseManagement;
    private PauseManager pauseManager;

    [SerializeField] private GameObject coinManagement;
    private CoinManager coinManager;

    private GameController() { instance = this; }
    public static GameController Instance { get => instance == null ? (instance = new GameController()) : instance; }

    public GameObject Player { get => player; }
    public GameState GameState_ { get => gameState; set => gameState = value; }

    public PlayerController GetPlayerController()
    {
        return player.GetComponent<PlayerController>();
    }

    private void Awake()
    {
        player = GameObject.Instantiate(playerTemplate, Position_Player_Spawn, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.RUNNING;

        Time.timeScale = 1f;
        if (!doDebug)
            debug.SetActive(false);

        timeManager = timeManagement.GetComponent<TimeManager>();
        gameOverManager = gameOverManagement.GetComponent<GameOverManager>();
        pauseManager = pauseManagement.GetComponent<PauseManager>();
        coinManager = coinManagement.GetComponent<CoinManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Gets called when player enters a new cannon to add time.
    public void ProcessBonusTime(float time)
    {
        timeManager.AddTime(time);
    }

    public void ProcessGameOver(GameOverManager.GameOverCause cause)
    {
        gameOverManager.ProcessGameOver(cause);
        Time.timeScale = 0f;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseButtonPressed()
    {
        pauseManager.PauseButtonPressed();
    }

    public void CoinCollected(int value)
    {
        coinManager.CoinCollected(value);
    }
}
