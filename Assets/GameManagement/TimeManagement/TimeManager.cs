using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Class that handles timer in game. All time variables in seconds.
public class TimeManager : MonoBehaviour
{
    [SerializeField] private Text timeText;

    [SerializeField] private float startingTime = 10f;
    private float currentTime;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        UpdateTimeText();
        if(currentTime <= 0f)
        {
            GameController.Instance.ProcessGameOver(GameOverManager.GameOverCause.OUT_OF_TIME);
        }
    }

    private void UpdateTimeText()
    {
        timeText.text = Math.Round(currentTime, 1).ToString("N1");
    }

    public void AddTime(float time)
    {
        currentTime += time;
    }
}
