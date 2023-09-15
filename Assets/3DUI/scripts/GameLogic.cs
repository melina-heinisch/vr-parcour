using System.Collections;
using System.Collections.Generic;
using _3DUI.scripts;
using _3DUI.scripts.keyboard;
using _3DUI.scripts.scoreboard;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class GameLogic : MonoBehaviour
{
    public VRHostSystem VRHostSystem;
    public Rigidbody rigidbodyObj;

    // Gameobjects need to be reset at some point
    public GameObject startBarrier;
    public GameObject endBarrier;
    
    // Gameobjects that need to be (de)activated at some point or that hold needed scripts
    public GameObject scoreboard;
    public GameObject keyboard;
    public GameObject startSaveButton;
    public GameObject nameInputField;
    public GameObject restartingInfoText;
    public GameObject resultUi;
    public GameObject restartTimerObj;
    public GameObject rightHand;
    
    // Audioclips to switch in case of easter Egg
    public AudioClip easterEgg;
    public AudioClip gameOver;

    // Bools that control game flow
    public bool isGameRunning = true;
    public bool isGameOver = false;
    public bool isWin = false;
    
    // Times for time challenge mode
    public float totalTime = 180;
    public float timeRemaining = 180;
    public bool timeChallengeActive = false;
    
    // Variables needed for automatic restart timer if game won
    private float restartTimerDuration = 10;
    
    // Text that needs to be set during the game
    public List<TextMeshProUGUI> timeTexts;
    public TextMeshProUGUI timeTextResult;
    public TextMeshProUGUI scoreboardList;

    // Components that are set at Start and accessed several times during the game
    private HandSwinging handSwinging;
    private Jumping jumping;
    private AudioSource audioSource;
    private ModifyRaycast modifyRaycast;
    private XRInteractorLineVisual xrInteractorLineVisual;
    private Timer timer;
    
    // Coroutine for restart timer at the end of game, in order to pause the coroutine if needed
    private Coroutine restartCoroutine;
    
    // To avoid one obstacle giving two time penalties
    private HashSet<TimePenaltyDetection> appliedPenalties = new();

    private void Start()
    {
        // Initialize all the components needed later
        handSwinging = VRHostSystem.getXROriginGameObject().GetComponent<HandSwinging>();
        jumping = VRHostSystem.getXROriginGameObject().GetComponent<Jumping>();
        audioSource = GetComponent<AudioSource>();
        modifyRaycast = rightHand.GetComponent<ModifyRaycast>();
        xrInteractorLineVisual = rightHand.GetComponent<XRInteractorLineVisual>();
        timer = restartTimerObj.GetComponent<Timer>();
        
        // Set time correctly for all elemets that show it in the scene
        timeRemaining = totalTime;
        string time = GetTimeInMinSec(timeRemaining);
        timeTexts.ForEach(elem => elem.text = time);

        

        //Add dummy data to scoreboard, if none is saved there
        var sbm = ScoreBoardManager.Instance;
        if (sbm.scoreBoard.Count == 0)
        {
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Lukas", 43.03381f));
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Meow", 44.99686f));
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Melina", 50.52271f));
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Lena", 54.79762f));
            sbm.AddScoreBoardEntry(new ScoreBoardEntry("Lukas", 56.89939f));
        }
        PopulateScoreboard();
        scoreboard.SetActive(false);
    }
    
    void Update()
    {
        if (isGameRunning)
        {
            if (timeChallengeActive)
            {
                // Decrease timer
                if (timeRemaining > 0)
                    timeRemaining -= Time.deltaTime;
                else
                {
                    // If timer if at 0 end game
                    isGameOver = true;
                    timeChallengeActive = false;
                    timeRemaining = 0;
                }

                // Update all elements that show time in the scene
                string time = GetTimeInMinSec(timeRemaining);
                timeTexts.ForEach(elem => elem.text = time);
            }

            // Trigger appropriate response to game events
            if (isGameOver)
                GameOver();
            else if (isWin)
                GameWon();
        }
    }

    void GameOver()
    {
        isGameRunning = false;
        audioSource.Play();
        StartCoroutine(RestartGame(1f, "Game Over - Restarting", 3f));
    }

    void GameWon()
    {
        isGameRunning = false;
        
        // Show UI at the end of the game
        string result = GetTimeInMinSec(totalTime - timeRemaining);
        timeTextResult.text = result;
        startSaveButton.SetActive(true);
        resultUi.SetActive(true);
        PopulateScoreboard();
        scoreboard.SetActive(true);
        
        // Set correct ray and show it for interaction
        modifyRaycast.setLongStraightRay();
        xrInteractorLineVisual.enabled = true;
        
        // Prevent movement of user
        handSwinging.enabled = false;
        jumping.enabled = false;
        
        // Start timer which restarts game automatically once its run out of time
        timer.StartTimer(restartTimerDuration);
        restartCoroutine = StartCoroutine(RestartGame(restartTimerDuration));
        
    }
    
    IEnumerator RestartGame(float wait, string infoText = "", float blackTime = 2f)
    {
        // Wait until timer has finished
        yield return new WaitForSeconds(wait);
        
        // Activate Fader by slowly fading to black
        Fader.FadeToBlack(infoText: infoText);
        yield return new WaitForSeconds(blackTime);
        
        // Force closing of helpmenu if it was active before
        HelpMenuController helpMenuController = FindObjectOfType<HelpMenuController>();
        if(helpMenuController) helpMenuController.Close();
        
        // Hide UI again
        resultUi.SetActive(false);
        scoreboard.SetActive(false);
        
        // Set correct ray for grabbing TP gun and do not show its line
        modifyRaycast.setShortStraightRay();
        xrInteractorLineVisual.enabled = false;
        
        // Reset time again and update texts accordingly
        timeRemaining = totalTime;
        string time = GetTimeInMinSec(timeRemaining);
        timeTexts.ForEach(elem => elem.text = time);
        
        // Reset all important game booleans
        isWin = false;
        isGameOver = false;
        timeChallengeActive = false;
        
        // Reset Time penalty
        foreach (var timePenaltyDetection in appliedPenalties)
        {
            timePenaltyDetection.Reset();
        }
        appliedPenalties.Clear();
        
        // Reset Barriers and audio
        startBarrier.GetComponent<StartParkourDetection>().Reset();
        endBarrier.GetComponent<EndParkourDetection>().Reset();
        audioSource.clip = gameOver;
        
        // Stop any motion of the player (e.g. from falling) and position the player at start again
        rigidbodyObj.velocity = Vector3.zero;
        VRHostSystem.getXROriginGameObject().transform.position = new Vector3(0, 3f, 0);
        VRHostSystem.getXROriginGameObject().transform.eulerAngles = new Vector3(0, 0, 0);
        
        // Enable Movement again
        handSwinging.enabled = true;
        jumping.enabled = true;
        
        // Start game again and fade to scene
        isGameRunning = true;
        Fader.FadeToScene();
    }
    
    // Returns formatted time String from float
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

    // Adjusts time in reaction to hitting wall
    public void AddTimePenalty(TimePenaltyDetection timePenaltyDetection)
    {
        var applied = appliedPenalties.Add(timePenaltyDetection);
        if (applied)
        {
            timeRemaining -= timePenaltyDetection.penalty;
        }
    }

    // Set the text of the scoreboard with all existing entries
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

    // Show keyboard and thus pause game to enter name into scoreboard
    public void ShowKeyboard()
    {
        // Prevent automatic restart
        if (restartCoroutine != null)
        {
            StopCoroutine(restartCoroutine);
            Debug.Log("stopped restart");
        }
        timer.StopTimer();
        
        // Change UI to show correct elements
        restartTimerObj.SetActive(false);
        startSaveButton.SetActive(false);
        nameInputField.SetActive(true);
        keyboard.SetActive(true);
        
        // Prepare keyboard manager to be ready for input
        var keyboardManager = keyboard.GetComponent<KeyboardManager>();
        keyboardManager.Reset();
        keyboardManager.EnterText = AddScoreBoardEntryAndRestart;
        
        // Update input based on typed letters
        var inputField = nameInputField.GetComponent<TMP_InputField>();
        inputField.text = "";
        keyboardManager.UpdateDisplay = typedContent => inputField.text = typedContent;
    }

    private void AddScoreBoardEntryAndRestart(string name)
    {
        // Check for easteregg
        if (name == "meow" || name == "Meow")
        {
            audioSource.clip = easterEgg;
            audioSource.Play();
        }
        
        AddScoreBoardEntry(name);
        
        // Activate Timer again and restart game
        restartingInfoText.SetActive(true);
        restartTimerObj.SetActive(true);
        restartCoroutine = StartCoroutine(RestartGame(restartTimerDuration));
        timer.StartTimer(restartTimerDuration);
    }
    
    private void AddScoreBoardEntry(string name)
    {
        // Add the item
        var entry = new ScoreBoardEntry(name, totalTime - timeRemaining);
        ScoreBoardManager.Instance.AddScoreBoardEntry(entry);
        
        // Hide elements used for scoreboard entry
        keyboard.SetActive(false);
        nameInputField.SetActive(false);
        
        //Reload Data in scoreboard
        PopulateScoreboard();
        PopulateScoreboard();
    }
}
