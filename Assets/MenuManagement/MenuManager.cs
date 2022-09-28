using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    private static readonly int Num_Levels = 15;

    [SerializeField] private GameObject menuUiManagement;
    private MenuUiManager menuUiManager;

    private void Start()
    {
        menuUiManager = menuUiManagement.GetComponent<MenuUiManager>();
        UpdateMenuUI();
    }

    private void UpdateMenuUI()
    {
        menuUiManager.DisplayHighscoreText(PlayerPrefs.GetInt(FileManager.HIGHSCORE, 0));
        menuUiManager.DisplayCoinsTotalText(PlayerPrefs.GetInt(FileManager.COINS, 0));
        menuUiManager.ColorLevelButtons(GetLevelCompletionVariables());
    }

    public void StartEndlessMode()
    {
        SceneManager.LoadScene("EndlessMode", LoadSceneMode.Single);
    }

    public void StartLevel(int level)
    {
        SceneManager.LoadScene("Level" + level, LoadSceneMode.Single);
    }

    private int[] GetLevelCompletionVariables()
    {
        int[] levelCompletionVariables = new int[Num_Levels];
        for(int i = 0; i < levelCompletionVariables.Length; ++i)
        {
            levelCompletionVariables[i] = PlayerPrefs.GetInt("Level" + (i + 1), 0);
        }
        return levelCompletionVariables;
    }

    public void ResetGameStats()
    {
        bool doReset = EditorUtility.DisplayDialog(
            "Are you sure?",
            "This will reset ALL game stats, including coins, gems, highscore and bought items. Do you really want to continue?",
            "Reset",
            "Cancel");

        if (!doReset)
            return;

        PlayerPrefs.SetInt(FileManager.HIGHSCORE, 0);
        PlayerPrefs.SetInt(FileManager.COINS, 0);
        PlayerPrefs.SetInt(FileManager.GEMS, 0);
        for(int i = 0; i < Num_Levels; ++i)
        {
            PlayerPrefs.SetInt("Level" + (i + 1), 0);
        }
        UpdateMenuUI();
    }
}
