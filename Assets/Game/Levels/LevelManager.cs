using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;

    [SerializeField] private int level;

    [SerializeField] private GameObject levelUiManagement;
    private LevelUiManager levelUiManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        levelUiManager = levelUiManagement.GetComponent<LevelUiManager>();
        GameController.Instance.GameMode_ = GameController.GameMode.LEVEL;
    }

    public static LevelManager Instance { get => instance; }

    public void OnLevelCompleted()
    {
        if(!IsLevelCompleted())
        {
            PlayerPrefs.SetInt("Level" + level, 1);
            int coinReward = level * 20;
            GameController.Instance.CoinCollected(coinReward);
            GameController.Instance.UpdateTotalCoins();
            levelUiManager.DisplayLevelCompletedUI(coinReward);
        }
        else
        {
            levelUiManager.DisplayLevelCompletedUI();
        }

        GameController.Instance.GameState_ = GameController.GameState.GAME_OVER;
        Time.timeScale = 0f;
    }

    private bool IsLevelCompleted()
    {
        return PlayerPrefs.GetInt("Level" + level, 0) != 0;
    }
}
