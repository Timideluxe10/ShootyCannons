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
    }

    public static LevelManager Instance { get => instance; }

    public void OnLevelCompleted()
    {
        PlayerPrefs.SetInt("Level" + level, 1);
        levelUiManager.DisplayLevelCompletedUI();
        GameController.Instance.GameState_ = GameController.GameState.GAME_OVER;
        Time.timeScale = 0f;
    }
}
