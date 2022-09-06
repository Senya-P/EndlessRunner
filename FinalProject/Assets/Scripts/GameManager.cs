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
    public Text scoreText;
    private static int score;
    public Text hightScoreText;
    private static int hightScore;

    void Start()
    {
        gameOver = false;
        numberOfCoins = 0;
        hightScore = PlayerPrefs.GetInt("HightScore", 0); // load hight score from user prefs, default is 0
    }

    // Update is called once per frame
    void Update()
    {
        score = (int)(Time.timeSinceLevelLoad * 10 + numberOfCoins * 10);
        if (score > hightScore)
            hightScore = score;
        scoreText.text = "Score: " + (score).ToString("0");  // score counter: playing time + collected coins
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            hightScoreText.text = "Hight Score: " + (hightScore).ToString("0");
            PlayerPrefs.SetInt("HightScore", hightScore);
            PlayerPrefs.Save();
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
