using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuUiManagement;
    private MenuUiManager menuUiManager;

    private void Start()
    {
        menuUiManager = menuUiManagement.GetComponent<MenuUiManager>();

        menuUiManager.DisplayHighscoreText(PlayerPrefs.GetInt(FileManager.HIGHSCORE, 0));
        menuUiManager.DisplayCoinsTotalText(PlayerPrefs.GetInt(FileManager.COINS, 0));
    }

    public void StartEndlessMode()
    {
        SceneManager.LoadScene("EndlessMode", LoadSceneMode.Single);
    }
}
