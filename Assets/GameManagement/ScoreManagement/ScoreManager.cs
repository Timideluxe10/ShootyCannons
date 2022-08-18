using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    private GameObject player;
    private float lastXPos; /* The last x position of the player to calculate a delta. */
    private float score;

    // Start is called before the first frame update
    void Start()
    {
        player = GameController.Instance.Player;
    }

    // Update is called once per frame
    void Update()
    {
        float currentXPos = player.transform.position.x;
        float deltaXPos = currentXPos - lastXPos;
        if (deltaXPos <= 0)
            return;
        else
        {
            score += deltaXPos;
            lastXPos = currentXPos;
            UpdateScorePanel();
        }
    }

    public void UpdateScorePanel()
    {
        scoreText.text = ((int)score).ToString();
    }

}
