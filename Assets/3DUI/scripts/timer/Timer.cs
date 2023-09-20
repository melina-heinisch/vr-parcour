using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class Timer : MonoBehaviour
{
    // Made with help of the following video: https://youtu.be/2MSMmPWedyg
    public GameObject timer;
    public GameObject radialProgressBar;
    // public bool timerIsActive;

    private Coroutine timerCoroutine;
    
    public void StartTimer(float duration)
    {
        // timerIsActive = true;
        timer.SetActive(true);
        radialProgressBar.GetComponent<CircularProgressBar>().ActivateCountdown(duration);
        timerCoroutine = StartCoroutine(EndTimer(duration));
    }

    public void StopTimer()
    {
        if(timerCoroutine != null)
            StopCoroutine(timerCoroutine);
    }

    IEnumerator EndTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
        // timerIsActive = false;
        timer.SetActive(false);
    }
}

