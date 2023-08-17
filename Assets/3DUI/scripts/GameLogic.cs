using System;
using System.Collections;
using System.Collections.Generic;
using _3DUI.scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class GameLogic : MonoBehaviour
{
    public VRHostSystem VRHostSystem;
    public GameObject rightHand;
    public Rigidbody rigidbodyObj;

    public GameObject startBarrier;
    public GameObject endBarrier;

    public bool isGameRunning = true;
    public float totalTime = 60;
    public float timeRemaining;
    public bool timerActive = false;
    public bool isGameOver = false;
    public bool isWin = false;
    public TextMeshProUGUI timeTextGoal;
    public GameObject resultPanel;
    public TextMeshProUGUI timeTextResult;
    public GameObject restartTimer;
    private float restartTimerDuration = 10;
    public TextMeshProUGUI scoreboardList;
    public GameObject scoreboardPanel;
    public GameObject keyboard;
    public GameObject startSaveButton;
    public GameObject nameInputField;
    public GameObject restartingInfoText;
    private Coroutine restartCoroutine;
    
    private void Start()
    {
        timeRemaining = totalTime;
        timeTextGoal.text = GetTimeInMinSec(timeRemaining);

        //Add dummy data
        var sbm = ScoreBoardManager.Instance;
        if (sbm.scoreBoard.Count == 0)
        {
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Luise", 9.68f));
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Lars", 7.68f));
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Laura", 8.68f));
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Leon", 7.98f));
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Lea", 8.38f));
        }
        PopulateScoreboard();
        scoreboardPanel.SetActive(false);
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
            timeTextGoal.text = GetTimeInMinSec(timeRemaining);
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
        //TODO: Why is reset after falling not working?
        StartCoroutine(RestartGame(2));
    }

    void GameWon()
    {
        isGameRunning = false;
        Debug.Log("Game Won");
        string result = GetTimeInMinSec(totalTime - timeRemaining);
        timeTextResult.text = result;
        startSaveButton.SetActive(true);
        resultPanel.SetActive(true);
        PopulateScoreboard();
        scoreboardPanel.SetActive(true);
        rightHand.GetComponent<XRInteractorLineVisual>().enabled = true;
        VRHostSystem.getXROriginGameObject().GetComponent<HandSwinging>().enabled = false;
        restartTimer.GetComponent<Timer>().StartTimer(restartTimerDuration);
        restartCoroutine = StartCoroutine(RestartGame(restartTimerDuration));
        
    }

    // Made with help of following tutorial: https://gamedevbeginner.com/how-to-make-countdown-timer-in-unity-minutes-seconds/
    string GetTimeInMinSec(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = (timeToDisplay % 1) * 1000;
        if(minutes >= 0 && seconds >= 0)
            return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds);
        else
            return string.Format("{0:00}:{1:00}:{2:000}", 0, 0, milliSeconds);
    }

    IEnumerator RestartGame(float wait)
    {
        yield return new WaitForSeconds(wait);
        resultPanel.SetActive(false);
        scoreboardPanel.SetActive(false);
        rightHand.GetComponent<XRInteractorLineVisual>().enabled = false;
        timeRemaining = totalTime;
        timeTextGoal.text = GetTimeInMinSec(timeRemaining);
        isWin = false;
        isGameOver = false;
        timerActive = false;
        rigidbodyObj.velocity = Vector3.zero;
        VRHostSystem.getXROriginGameObject().transform.position = new Vector3(0, 3f, 0);
        VRHostSystem.getXROriginGameObject().transform.eulerAngles = new Vector3(0, 0, 0);
        VRHostSystem.getXROriginGameObject().GetComponent<HandSwinging>().enabled = true;
        startBarrier.GetComponent<StartParkourDetection>().Reset();
        endBarrier.GetComponent<EndParkourDetection>().Reset();
        isGameRunning = true;
    }
    
    public List<ScoreBoardEntry> GetSortedScoreBoardEntries(List<ScoreBoardEntry> entries)
    {
        entries.Sort((s1,s2) => s1.CompareTo(s2));
        return entries;
    }

    private void PopulateScoreboard()
    {
        var scoreboard = ScoreBoardManager.Instance.scoreBoard;
        string content = "";
        int len = scoreboard.Count <= 10 ? scoreboard.Count : 11;
        for (int i = 1; i < len; i++)
        {
            ScoreBoardEntry sb = scoreboard[i-1];
            content += i + " - " + sb.Name + "\t \t" + GetTimeInMinSec(sb.Time) + "\n";
        }

        scoreboardList.text = content;
    }

    public void ShowKeyboard()
    {
        if (restartCoroutine != null)
        {
            StopCoroutine(restartCoroutine);
            Debug.Log("stopped restart");
        }
        restartTimer.GetComponent<Timer>().StopTimer();
        restartTimer.SetActive(false);
        startSaveButton.SetActive(false);
        nameInputField.SetActive(true);
        keyboard.SetActive(true);
        var kbm = keyboard.GetComponent<KeyboardManager>();
        kbm.Reset();
        kbm.EnterText = AddScoreBoardEntryAndRestart;
        var nif = nameInputField.GetComponent<TMP_InputField>();
        nif.text = "";
        kbm.UpdateDisplay = typedContent => nif.text = typedContent;
    }

    private void AddScoreBoardEntryAndRestart(string name)
    {
        restartingInfoText.SetActive(true);
        restartTimer.SetActive(true);
        AddScoreBoardEntry(name);
        StartCoroutine(RestartGame(restartTimerDuration));
        restartTimer.GetComponent<Timer>().StartTimer(restartTimerDuration);
    }
    
    private void AddScoreBoardEntry(string name)
    {
        var entry = new ScoreBoardEntry(name, totalTime - timeRemaining);
        ScoreBoardManager.Instance.AddScoreBoardEntry(entry);
        keyboard.SetActive(false);
        nameInputField.SetActive(false);
        PopulateScoreboard();
    }
}
