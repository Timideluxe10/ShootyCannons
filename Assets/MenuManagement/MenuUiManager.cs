using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highscoreText;

    [SerializeField] private TextMeshProUGUI coinsTotalText;

    public void DisplayHighscoreText(int highscore)
    {
        highscoreText.text = "Highscore: " + highscore;
    }

    public void DisplayCoinsTotalText(int coinsTotal)
    {
        coinsTotalText.text = "" + coinsTotal;
    }
}
