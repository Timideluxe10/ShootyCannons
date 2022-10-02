using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUiManager : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletionPanel;

    [SerializeField] private TextMeshProUGUI coinRewardText;

    public void DisplayLevelCompletedUI(int coinReward)
    {
        coinRewardText.text = "Level Reward:\n" + coinReward + " Coins!";
        levelCompletionPanel.SetActive(true);
    }

    public void DisplayLevelCompletedUI()
    {
        coinRewardText.text = "";
        levelCompletionPanel.SetActive(true);
    }
}
