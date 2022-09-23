using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUiManager : MonoBehaviour
{
    [SerializeField] private GameObject levelCompletionPanel;

    public void DisplayLevelCompletedUI()
    {
        levelCompletionPanel.SetActive(true);
    }
}
