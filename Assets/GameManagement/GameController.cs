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
    private static readonly float Gravity = 15f;

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

    [SerializeField] private GameObject effectManagement;
    private EffectManager effectManager;

    [SerializeField] private GameObject itemManagement;
    private ItemManager itemManager;


    private float timeScaleWhenRunning;

    public static GameController Instance { get => instance; }

    public GameObject Player { get => player; }
    public GameState GameState_ { get => gameState; set => gameState = value; }
    public float TimeScaleWhenRunning { get => timeScaleWhenRunning; }

    public PlayerController GetPlayerController()
    {
        return player.GetComponent<PlayerController>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        Physics.gravity = new Vector3(0, -Gravity, 0);

        player = GameObject.Instantiate(playerTemplate, Position_Player_Spawn, Quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.RUNNING;

        Time.timeScale = 1f;
        timeScaleWhenRunning = 1f;

        if (!doDebug)
            debug.SetActive(false);

        timeManager = timeManagement.GetComponent<TimeManager>();
        gameOverManager = gameOverManagement.GetComponent<GameOverManager>();
        pauseManager = pauseManagement.GetComponent<PauseManager>();
        coinManager = coinManagement.GetComponent<CoinManager>();
        effectManager = effectManagement.GetComponent<EffectManager>();
        itemManager = itemManagement.GetComponent<ItemManager>();
    }

    public void CollectableItemCollected(GameObject collectableItem)
    {
        itemManager.Collect(collectableItem);
    }

    public void OnItemUse(ItemController itemController)
    {
        itemManager.OnUse(itemController);
    }

    public void OnItemExpire(ItemController itemController)
    {
        itemManager.OnExpire(itemController);
    }

    public void UseItem()
    {
        itemManager.UseItem();
    }

    public void DiscardItem()
    {
        itemManager.DiscardItem();
    }

    public void SetDrawTrajectory(bool doDrawTrajectory)
    {
        effectManager.SetDrawTrajectory(doDrawTrajectory);
    }

    public void SetTimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        timeScaleWhenRunning = timeScale;
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
