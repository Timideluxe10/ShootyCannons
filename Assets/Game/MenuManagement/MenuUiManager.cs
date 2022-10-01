using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highscoreText;

    [SerializeField] private TextMeshProUGUI coinsTotalText;

    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private Button[] levelButtons;
    [SerializeField] private Color levelCompleteColor;
    [SerializeField] private Color levelIncompleteColor;

    public void DisplayHighscoreText(int highscore)
    {
        highscoreText.text = "Highscore: " + highscore;
    }

    public void DisplayCoinsTotalText(int coinsTotal)
    {
        coinsTotalText.text = "" + coinsTotal;
    }

    public void ToggleOptionsPanelActive()
    {
        optionsPanel.SetActive(!optionsPanel.activeInHierarchy);
    }

    public void ColorLevelButtons(int[] levelCompletionVariables)
    {
        for(int i = 0; i < levelButtons.Length; ++i)
        {
            var colors = levelButtons[i].GetComponent<Button>().colors;
            colors.normalColor = levelCompletionVariables[i] == 0 ? levelIncompleteColor : levelCompleteColor;
            colors.highlightedColor = colors.normalColor;
            colors.pressedColor = colors.normalColor;
            levelButtons[i].GetComponent<Button>().colors = colors;
        }
    }
}
