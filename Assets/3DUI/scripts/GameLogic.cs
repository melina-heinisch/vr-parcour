using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public bool isGameRunning = true;
    public float timeRemaining = 60;
    public bool timerActive = false;
    public bool isGameOver = false;
    public bool isWin = false;
    public TextMeshProUGUI timeText;

    private void Start()
    {
        GetTimeInMinSec(timeRemaining);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {
          if (timerActive)
          {
              
            if (timeRemaining > 0)
                timeRemaining -= Time.deltaTime;
            else
            {
                isGameOver = true;
                timerActive = false;
                timeRemaining = 0;
            }
            GetTimeInMinSec(timeRemaining);
          }

          if (isGameOver)
              GameOver();
          else if (isWin)
              GameWon();  
        }
        
    }

    void GameOver()
    {
        isGameRunning = false;
        Debug.Log("Game Over");
    }

    void GameWon()
    {
        isGameRunning = false;
        Debug.Log("Game Won");
    }

    void GetTimeInMinSec(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if(minutes >= 0 && seconds >= 0)
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
