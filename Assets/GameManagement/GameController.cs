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

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject debug;
    [SerializeField] private bool doDebug;

    [SerializeField] private GameObject timeManagement;
    private TimeManager timeManager;

    [SerializeField] private GameObject gameOverManagement;
    private GameOverManager gameOverManager;

    [SerializeField] private GameObject pauseManagement;
    private PauseManager pauseManager;

    private GameController() { instance = this; }
    public static GameController Instance { get => instance == null ? (instance = new GameController()) : instance; }

    public GameObject Player { get => player; }
    public GameState GameState_ { get => gameState; set => gameState = value; }

    public PlayerController GetPlayerController()
    {
        return player.GetComponent<PlayerController>();
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
}
