using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public static readonly string HIGHSCORE = "Highscore";
    public static readonly string COINS = "Coins";

    public void UpdateHighscore()
    {
        int playerScore = (int) GameController.Instance.GetScore();
        if (GameController.Instance.GetScore() > PlayerPrefs.GetInt(HIGHSCORE, 0))
            PlayerPrefs.SetInt(HIGHSCORE, playerScore);
    }

    public void UpdateTotalCoins()
    {
        int playerCoins = GameController.Instance.GetCollectedCoins();
        PlayerPrefs.SetInt(COINS, PlayerPrefs.GetInt(COINS, 0) + playerCoins);
    }
}
