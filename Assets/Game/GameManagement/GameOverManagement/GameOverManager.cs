using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text gameOverMessageText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private float minDistanceToLastCannonForGameOver = 15f;

    [SerializeField] private AudioClip gameOverAudioClip;

    private static Dictionary<GameOverCause, string> gameOverMessages;

    private GameObject player;
    private PlayerController playerController;

    public enum GameOverCause
    {
        OUT_OF_TIME, PLAYER_FELL
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
        InitGameOverMessages();
        player = GameController.Instance.Player;
        playerController = player.GetComponent<PlayerController>();
    }

    private void InitGameOverMessages()
    {
        gameOverMessages = new Dictionary<GameOverCause, string>()
        {
            { GameOverCause.OUT_OF_TIME, "You ran out of time. Too old, too slow."},
            { GameOverCause.PLAYER_FELL, "You missed a cannon and fell down."}
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.IsGameRunning() && CheckForCriticalPlayerPosition())
            GameController.Instance.ProcessGameOver(GameOverCause.PLAYER_FELL);
    }

    private bool CheckForCriticalPlayerPosition()
    {
        GameObject lastCannon = playerController.LastCannon;
        if(lastCannon != null)
        {
            float yPositionLastCannon = lastCannon.transform.position.y;
            float yPositionPlayer = player.transform.position.y;
            if (yPositionLastCannon - yPositionPlayer >= minDistanceToLastCannonForGameOver)
                return true;
        }
        return false;
    }

    public void ProcessGameOver(GameOverCause cause)
    {
        gameOverPanel.SetActive(true);
        gameOverMessageText.text = gameOverMessages[cause];
        scoreText.text = "Score: " + (int) GameController.Instance.GetScore();
        GameController.Instance.PlaySound(gameOverAudioClip, player.transform.position);
        if(GameController.Instance.GameMode_ == GameController.GameMode.ENDLESS)
        {
            GameController.Instance.UpdateHighscore();
            GameController.Instance.UpdateTotalCoins();
        }
    }
}
