using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool gameOver;
    public static bool isStarted = true;
    public GameObject gameOverPanel;
    public GameObject startGamePanel;
    public static int numberOfCoins;
    public Text score;

    void Start()
    {
        gameOver = false;
        numberOfCoins = 0;
    }

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + (Time.timeSinceLevelLoad * 10 + numberOfCoins * 10).ToString("0");  // score counter: playing time + collected coins
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
        else
        {
            if (isStarted) // if the game has just started -> to show the menu
            {
                Time.timeScale = 0;
                startGamePanel.SetActive(true);
            }
            else  // if the player is replaying
            {
                startGamePanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
}
