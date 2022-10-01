using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private static readonly string Pause_Symbol = "II";
    private static readonly string Unpause_Symbol = ">";

    [SerializeField] private Text pauseText;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PauseButtonPressed()
    {
        if (!isPaused)
            PauseGame();
        else
            UnpauseGame();
    }

    public void PauseGame()
    {
        pauseText.text = Unpause_Symbol;
        Time.timeScale = 0f;
        isPaused = true;
        GameController.Instance.GameState_ = GameController.GameState.PAUSED;
    }

    public void UnpauseGame()
    {
        pauseText.text = Pause_Symbol;
        Time.timeScale = GameController.Instance.TimeScaleWhenRunning;
        isPaused = false;
        GameController.Instance.GameState_ = GameController.GameState.RUNNING;
    }


}
