using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuUiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highscoreText;

    [SerializeField] private TextMeshProUGUI coinsTotalText;

    [SerializeField] private TextMeshProUGUI gemsTotalText;

    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private Button levelButtonTemplate;
    [SerializeField] private Sprite levelIncompleteButtonSprite;
    [SerializeField] private Sprite levelCompleteButtonSprite;
    [SerializeField] private int numLevels;
    private Button[] levelButtons;

    private MenuManager menuManager;

    public MenuManager MenuManager { set => menuManager = value; }

    public void InitLevelButtons(int[] levelCompletionVariables)
    {
        levelButtons = new Button[numLevels];
        int levelNumber = 0;
        for(int i = 0; i < 3; ++i)
        {
            for(int j = 0; j < numLevels / 3; ++j)
            {
                // levelButtons[levelNumber++] = GameObject.Instantiate(levelButtonTemplate, new Vector3(j * 100 + (j+1) * 10, i * 100 + (i+1) * 10, 0), levelButtonTemplate.transform.rotation, levelsPanel.transform);

                Button levelButton = GameObject.Instantiate(levelButtonTemplate);
                levelButton.transform.SetParent(levelsPanel.transform);
                levelButton.transform.localScale = levelButtonTemplate.transform.localScale;
                levelButton.transform.localPosition = levelButtonTemplate.transform.localPosition + new Vector3(j * 100 + (j) * 10, i * -100 + (i) * -10, 0)
                    - new Vector3(levelsPanel.GetComponent<RectTransform>().rect.width / 2, -levelsPanel.GetComponent<RectTransform>().rect.height / 2, 0);
                levelButtons[levelNumber] = levelButton;

                ++levelNumber;
                levelButton.name = "Level" + levelNumber;
                levelButton.GetComponentInChildren<TMP_Text>().text = levelNumber.ToString();
                int levelToStart = levelNumber;
                levelButton.onClick.AddListener(() => menuManager.StartLevel(levelToStart));
            }
        }
        ColorLevelButtons(levelCompletionVariables);
    }

    public void DisplayHighscoreText(int highscore)
    {
        highscoreText.text = "Highscore: " + highscore;
    }

    public void DisplayCoinsTotalText(int coinsTotal)
    {
        coinsTotalText.text = "" + coinsTotal;
    }

    public void DisplayGemsTotalText(int gemsTotal)
    {
        gemsTotalText.text = "" + gemsTotal;
    }

    public void ToggleOptionsPanelActive()
    {
        optionsPanel.SetActive(!optionsPanel.activeInHierarchy);
    }

    public void ColorLevelButtons(int[] levelCompletionVariables)
    {
        if (levelButtons == null)
            return;
        for(int i = 0; i < levelButtons.Length; ++i)
        {
            if(levelCompletionVariables[i] != 0)
            {
                levelButtons[i].GetComponentInChildren<Image>().sprite = levelCompleteButtonSprite;
            }
            else
                levelButtons[i].GetComponentInChildren<Image>().sprite = levelIncompleteButtonSprite;
        }
    }
}
