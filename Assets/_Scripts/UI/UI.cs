using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class UI : MonoBehaviour
{
    [Header("Score")]
    public TextMeshProUGUI scoreText;
    private int Score = 5; 
    private int scoreCount = 0;

    [Header("TIme")]
    public float timeRemaining = 120f; // 2 minutes
    public TextMeshProUGUI timerText;

    public bool isRunning;

    void Update()
    {
        if (scoreCount >= Score && timeRemaining>0) // If Player collects all points then go to win state
            GameManager.Instance.OnLevelComplete();

        if (!isRunning) return;


        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            timeRemaining = 0;
            isRunning = false;
            GameManager.Instance.OnTimeUp(); //If timer ends before collection of points go to Game Over State
        }
    }

    //TIME CALCULATUON
    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    //SET SCORE
    public void setScore()
    {
        scoreCount++;
        scoreText.text = string.Format("Score : {0}", scoreCount);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }


}
